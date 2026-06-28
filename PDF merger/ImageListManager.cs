using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_merger
{
    public class ImageListManager
    {
        private readonly List<ImageItem> _items = new List<ImageItem>();

        public IReadOnlyList<ImageItem> Items => _items;

        // New enum to distinguish why a file was not added
        public enum AddResult
        {
            Success,
            Duplicate,
            InvalidImage,
            Error
        }

        public AddResult TryAddImage(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return AddResult.Error;

            if (!File.Exists(filePath))
                return AddResult.Error;

            if (_items.Any(x => string.Equals(x.FilePath, filePath, StringComparison.OrdinalIgnoreCase)))
                return AddResult.Duplicate;

            if (!IsSupportedImage(filePath))
                return AddResult.InvalidImage;

            _items.Add(new ImageItem { FilePath = filePath });
            return AddResult.Success;
        }

        public void RemoveAtIndices(IEnumerable<int> indices)
        {
            var ordered = indices.Distinct().OrderByDescending(x => x).ToList();

            foreach (var index in ordered)
            {
                if (index >= 0 && index < _items.Count)
                    _items.RemoveAt(index);
            }
        }

        public void MoveBlockUp(IEnumerable<int> selectedIndices)
        {
            MoveBlock(selectedIndices, GetInsertionIndexForMoveUp(selectedIndices));
        }

        public void MoveBlockDown(IEnumerable<int> selectedIndices)
        {
            MoveBlock(selectedIndices, GetInsertionIndexForMoveDown(selectedIndices));
        }

        public void MoveBlockToTop(IEnumerable<int> selectedIndices)
        {
            MoveBlock(selectedIndices, 0);
        }

        public void MoveBlockToBottom(IEnumerable<int> selectedIndices)
        {
            MoveBlock(selectedIndices, _items.Count);
        }

        public void MoveBlockToPosition(IEnumerable<int> selectedIndices, int position1Based)
        {
            int targetIndex = Math.Max(0, Math.Min(position1Based - 1, _items.Count));
            MoveBlock(selectedIndices, targetIndex);
        }

        public void MoveBlockBeforeOriginalIndex(IEnumerable<int> selectedIndices, int originalTargetIndex)
        {
            var indexes = selectedIndices.Distinct().OrderBy(x => x).ToList();
            if (indexes.Count == 0)
                return;

            var selectedSet = new HashSet<int>(indexes);
            var moved = indexes.Select(i => _items[i]).ToList();

            var remaining = _items
                .Where((item, idx) => !selectedSet.Contains(idx))
                .ToList();

            int insertionIndex = originalTargetIndex - indexes.Count(i => i < originalTargetIndex);
            insertionIndex = Math.Max(0, Math.Min(insertionIndex, remaining.Count));

            remaining.InsertRange(insertionIndex, moved);
            ReplaceItems(remaining);
        }

        public void SortAscending()
        {
            _items.Sort((a, b) => NaturalSortComparer.Compare(a.FileName, b.FileName));
        }

        public void SortDescending()
        {
            _items.Sort((a, b) => NaturalSortComparer.Compare(b.FileName, a.FileName));
        }

        private void MoveBlock(IEnumerable<int> selectedIndices, int insertionIndex)
        {
            var indexes = selectedIndices.Distinct().OrderBy(x => x).ToList();
            if (indexes.Count == 0)
                return;

            var selectedSet = new HashSet<int>(indexes);
            var moved = indexes.Select(i => _items[i]).ToList();

            var remaining = _items
                .Where((item, idx) => !selectedSet.Contains(idx))
                .ToList();

            insertionIndex = Math.Max(0, Math.Min(insertionIndex, remaining.Count));
            remaining.InsertRange(insertionIndex, moved);

            ReplaceItems(remaining);
        }

        private int GetInsertionIndexForMoveUp(IEnumerable<int> selectedIndices)
        {
            var indexes = selectedIndices.Distinct().OrderBy(x => x).ToList();
            if (indexes.Count == 0)
                return 0;

            return Math.Max(0, indexes.First() - 1);
        }

        private int GetInsertionIndexForMoveDown(IEnumerable<int> selectedIndices)
        {
            var indexes = selectedIndices.Distinct().OrderBy(x => x).ToList();
            if (indexes.Count == 0)
                return 0;

            return Math.Min(_items.Count, indexes.First() + 1);
        }

        private void ReplaceItems(List<ImageItem> newItems)
        {
            _items.Clear();
            _items.AddRange(newItems);
        }

        private bool IsSupportedImage(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();

            switch (ext)
            {
                case ".bmp":
                case ".dib":
                case ".rle":
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                case ".png":
                case ".gif":
                case ".tif":
                case ".tiff":
                    break;
                default:
                    return false;
            }

            try
            {
                using (var img = Image.FromFile(filePath))
                {
                    return img.Width > 0 && img.Height > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
