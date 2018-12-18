using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityWithExistingDatabase.Models.ViewModels.Task
{
    public class AssignedTasksViewModel
    {
        public Dictionary<AssignedTaskViewModel, IEnumerable<AssignedUserViewModel>> UsersByTask { get; set; }
            = new Dictionary<AssignedTaskViewModel, IEnumerable<AssignedUserViewModel>>();
    }

    public class CreateViewModel
    {
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }

    public class CreateMilestoneViewModel
    {
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public bool Finished { get; set; }
    }

    public class BindAssignedUsersViewModel
    {
        public IEnumerable<int> Users { get; set; }
        public int TaskId { get; set; }
    }

    public class AssignUsersViewModel {
        public IEnumerable<AssignedUserViewModel> AssignedUsers { get; set; }
        public int TaskId { get; set; }
    }

    public class AssignedUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class AssignedTaskViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}