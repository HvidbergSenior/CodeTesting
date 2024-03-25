using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderExtraWork : ValueObject
    {
        public string Value { get; private set;}
        
        private ProjectFolderExtraWork()
        {
            Value = String.Empty;
        }

        private ProjectFolderExtraWork(string value)
        {
            Value = value;
        }

        private static ProjectFolderExtraWork Create(string value)
        {
            return new ProjectFolderExtraWork(value);
        }

        public static ProjectFolderExtraWork Empty()
        {
            return new ProjectFolderExtraWork(String.Empty);
        }

        public static ProjectFolderExtraWork ExtraWork()
        {
            return Create("ExtraWork");
        }

        public static ProjectFolderExtraWork Normal()
        {
            return Create("NormalWork");
        }

        public bool IsExtraWork()
        {
            return this == ExtraWork();
        }
    }
}
