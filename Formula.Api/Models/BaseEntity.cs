using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TrackUniversal2.Entities
{
    public class BaseEntity : IBaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedById { get; set; }
        public Guid? UpdatedById { get; set; }

        // [ForeignKey("CreatedById")]
        // public IdentityUser CreatedBy { get; set; }
        // [ForeignKey("UpdatedById")]
        // public IdentityUser? UpdatedBy { get; set; }
    }
}