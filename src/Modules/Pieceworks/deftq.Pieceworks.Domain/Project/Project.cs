using System.Collections.ObjectModel;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project.Company;
using deftq.Pieceworks.Domain.projectCompensation;
using FluentValidation;

namespace deftq.Pieceworks.Domain.project;

public sealed class Project : Entity
{
    public ProjectId ProjectId { get; private set; }
    public ProjectName ProjectName { get; private set; }
    public ProjectNumber ProjectNumber { get; private set; }
    public ProjectDescription ProjectDescription { get; private set; }
    public ProjectPieceWorkNumber ProjectPieceWorkNumber { get; private set; }
    public ProjectOrderNumber ProjectOrderNumber { get; private set; }
    public ProjectOwner ProjectOwner { get; private set; }
    public ProjectPieceworkType ProjectPieceworkType { get; private set; }
    public ProjectLumpSumPayment ProjectLumpSumPaymentDkr { get; private set; }
    public ProjectStartDate ProjectStartDate { get; private set; }
    public ProjectEndDate ProjectEndDate { get; private set; }
    public ProjectCreatedBy ProjectCreatedBy { get; private set; }
    public ProjectCreatedTime ProjectCreatedTime { get; private set; }
    public ProjectCompany ProjectCompany { get; private set; }

    // Project participants
    private IList<ProjectParticipant> _projectParticipants;

    public IReadOnlyList<ProjectParticipant> ProjectParticipants
    {
        get => new ReadOnlyCollection<ProjectParticipant>(_projectParticipants);
        private set => _projectParticipants = new List<ProjectParticipant>(value);
    }

    // Project managers
    private IList<ProjectManager> _projectManagers;

    public IReadOnlyList<ProjectManager> ProjectManagers
    {
        get => new ReadOnlyCollection<ProjectManager>(_projectManagers);
        private set => _projectManagers = new List<ProjectManager>(value);
    }

    private Project()
    {
        Id = Guid.NewGuid();
        ProjectId = ProjectId.Create(Id);
        ProjectName = ProjectName.Empty();
        ProjectNumber = ProjectNumber.Empty();
        ProjectDescription = ProjectDescription.Empty();
        ProjectPieceWorkNumber = ProjectPieceWorkNumber.Empty();
        ProjectOrderNumber = ProjectOrderNumber.Empty();
        ProjectOwner = ProjectOwner.Empty();
        _projectParticipants = new List<ProjectParticipant>();
        _projectManagers = new List<ProjectManager>();
        ProjectPieceworkType = ProjectPieceworkType.TwelveTwo;
        ProjectLumpSumPaymentDkr = ProjectLumpSumPayment.Empty();
        ProjectStartDate = ProjectStartDate.Empty();
        ProjectEndDate = ProjectEndDate.Empty();
        ProjectCreatedBy = ProjectCreatedBy.Empty();
        ProjectCreatedTime = ProjectCreatedTime.Empty();
        ProjectCompany = ProjectCompany.Empty();
    }

    private Project(ProjectId projectId, ProjectName projectName, ProjectNumber projectNumber, ProjectPieceWorkNumber projectPieceWorkNumber,
        ProjectOrderNumber projectOrderNumber, ProjectDescription projectDescription, ProjectOwner projectOwner,
        ProjectPieceworkType projectPieceworkType, ProjectLumpSumPayment projectLumpSumPaymentDkr, ProjectStartDate projectStartDate,
        ProjectEndDate projectEndDate, ProjectCreatedBy projectCreatedBy, ProjectCreatedTime projectCreatedTime, ProjectCompany projectCompany)
    {
        Id = projectId.Value;
        ProjectId = projectId;
        ProjectName = projectName;
        ProjectNumber = projectNumber;
        ProjectDescription = projectDescription;
        ProjectPieceWorkNumber = projectPieceWorkNumber;
        ProjectOrderNumber = projectOrderNumber;
        ProjectOwner = projectOwner;
        _projectParticipants = new List<ProjectParticipant>();
        _projectManagers = new List<ProjectManager>();
        ProjectPieceworkType = projectPieceworkType;
        ProjectLumpSumPaymentDkr = projectLumpSumPaymentDkr;
        ProjectStartDate = projectStartDate;
        ProjectEndDate = projectEndDate;
        ProjectCreatedBy = projectCreatedBy;
        ProjectCreatedTime = projectCreatedTime;
        ProjectCompany = projectCompany;
    }

    public static Project Create(ProjectId projectId, ProjectName projectName, ProjectNumber projectNumber,
        ProjectPieceWorkNumber projectPieceWorkNumber, ProjectOrderNumber projectOrderNumber, ProjectDescription projectDescription,
        ProjectOwner projectOwner,
        ProjectPieceworkType projectPieceworkType, ProjectLumpSumPayment projectLumpSumPaymentDkr, ProjectStartDate projectStartDate,
        ProjectEndDate projectEndDate, ProjectCreatedBy projectCreatedBy, ProjectCreatedTime projectCreatedTime, ProjectCompany projectCompany)
    {
        var project = new Project(projectId, projectName, projectNumber, projectPieceWorkNumber, projectOrderNumber, projectDescription, projectOwner,
            projectPieceworkType, projectLumpSumPaymentDkr, projectStartDate, projectEndDate, projectCreatedBy, projectCreatedTime, projectCompany);
        project.AddDomainEvent(ProjectCreatedDomainEvent.Create(projectId, projectName, projectDescription, projectOwner, projectPieceworkType));
        return project;
    }

    public void ChangeName(ProjectName name)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (name == this.ProjectName)
        {
            return;
        }

        this.ProjectName = name;
        AddDomainEvent(ProjectNameUpdatedDomainEvent.Create(ProjectId));
    }

    public void ChangeDescription(ProjectDescription description)
    {
        if (ProjectDescription == description)
        {
            return;
        }

        ProjectDescription = description;
        AddDomainEvent(ProjectDescriptionUpdatedDomainEvent.Create(ProjectId));
    }

    public void RemoveProject()
    {
        AddDomainEvent(ProjectRemovedDomainEvent.Create(ProjectId));
    }

    public bool IsOwner(Guid userId)
    {
        return ProjectOwner.Id == userId;
    }

    public bool IsParticipant(Guid userId)
    {
        return _projectParticipants.Any(p => p.Id == userId);
    }

    public bool IsProjectManager(Guid userId)
    {
        return _projectManagers.Any(p => p.Id == userId);
    }

    public void AddProjectParticipant(ProjectParticipant participant)
    {
        if (_projectParticipants.All(p => p.Id != participant.Id))
        {
            _projectParticipants.Add(participant);
            AddDomainEvent(ProjectParticipantAddedDomainEvent.Create(ProjectId, participant));
        }
    }

    public void AddProjectManager(ProjectManager projectManager)
    {
        if (_projectManagers.All(p => p.Id != projectManager.Id))
        {
            _projectManagers.Add(projectManager);
            AddDomainEvent(ProjectManagerAddedDomainEvent.Create(ProjectId, projectManager));
        }
    }

    public ProjectParticipant GetParticipant(Guid userId)
    {
        return ProjectParticipants.First(p => p.Id == userId);
    }
    
    public void UpdateLumpSumPayment(ProjectLumpSumPayment lumpSumPayment)
    {
        if (ProjectPieceworkType != ProjectPieceworkType.TwelveTwo)
        {
            throw new ValidationException("incorrect piecework type");
        }

        ProjectLumpSumPaymentDkr = lumpSumPayment;
    }

    public void UpdateProjectType(ProjectPieceworkType projectPieceworkType, ProjectLumpSumPayment projectLumpSumPayment)
    {
        if (projectPieceworkType == ProjectPieceworkType.TwelveTwo)
        {
            if (projectLumpSumPayment == ProjectLumpSumPayment.Empty())
            {
                throw new ValidationException("lump sum payment cannot be empty");
            }

            ProjectLumpSumPaymentDkr = projectLumpSumPayment;
            ProjectPieceworkType = projectPieceworkType;
        }
        else
        {
            ProjectPieceworkType = projectPieceworkType;
            ProjectLumpSumPaymentDkr = ProjectLumpSumPayment.Empty();
        }
    }

    public void UpdateProjectDates(ProjectStartDate requestStartDate, ProjectEndDate requestEndDate)
    {
        ProjectStartDate = requestStartDate;
        ProjectEndDate = requestEndDate;
    }

    public void ChangePieceWorkNumber(ProjectPieceWorkNumber projectPieceWorkNumber)
    {
        ProjectPieceWorkNumber = projectPieceWorkNumber;
    }

    public void ChangeOrderNumber(ProjectOrderNumber projectOrderNumber)
    {
        ProjectOrderNumber = projectOrderNumber;
    }

    public void UpdateProjectCompany(ProjectCompany projectCompany)
    {
        ProjectCompany = projectCompany;
    }
}
