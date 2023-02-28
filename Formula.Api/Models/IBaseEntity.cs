using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TrackUniversal2.Entities
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
        string Name { get; set; }
        DateTimeOffset CreatedDate { get; set; }
        DateTimeOffset? UpdatedDate { get; set; }
        bool IsActive { get; set; }
        Guid CreatedById { get; set; }
        Guid? UpdatedById { get; set; }
        // IdentityUser CreatedBy { get; set; }
        // IdentityUser? UpdatedBy { get; set; }
    }
}