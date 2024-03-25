using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement
{
    public sealed class FolderValueInheritStatus : ValueObject
    {
        public string Value { get; private set;}
        
        private FolderValueInheritStatus()
        {
            Value = String.Empty;
        }

        private FolderValueInheritStatus(string value)
        {
            Value = value;
        }

        private static FolderValueInheritStatus Create(string value)
        {
            return new FolderValueInheritStatus(value);
        }

        public static FolderValueInheritStatus Empty()
        {
            return new FolderValueInheritStatus(String.Empty);
        }

        public static FolderValueInheritStatus Inherit()
        {
            return Create("inherit");
        }

        public static FolderValueInheritStatus Overwrite()
        {
            return Create("overwrite");
        }

        public bool IsOverwritten()
        {
            return this == Overwrite();
        }
        
        public bool IsInherited()
        {
            return !IsOverwritten();
        }
    }
}
