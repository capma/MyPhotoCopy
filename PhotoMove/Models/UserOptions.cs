using System.Collections.Generic;

namespace PhotoMove.Models
{
    public class UserOptions
    {
        public string selectedFolderWithPhotosToProcess { get; set; } = string.Empty;
        public string selectedDestinationFolder { get; set; } = string.Empty;
        public string selectedDuplicatesFolder { get; set; } = string.Empty;
        public string selectedFolderForFilesWithNoExifDateCreated { get; set; } = string.Empty;
        public int selectedHowFolderAreStructured { get; set; }
        public int selectedHowFilesAreDuplicates { get; set; }
        public int selectedHowToMoveOrCopyExistedFilesInTheDestinationFolder { get; set; }
        public bool checkedUseFileDateToMoveOrCopyToStructureFolder { get; set; }
        public bool checkedCopyOrMoveFilesWithNoExifDateCreatedToThisFolder { get; set; }
        public bool checkedIncludeSubFolder { get; set; }
        public List<string> selectedFileTypes { get; set; } = new();
        public List<string> selectedCameraModels { get; set; } = new();
        public bool checkedUseDashesInFolderNames { get; set; }
        public bool checkedNoSeperator { get; set; }
        public bool checkedUseUnderscoresInFolderNames {  get; set; }

    }
}
