using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkLinq.Models
{
    [Table("person_contacts")]
    public class PersonContact
    {
        [Key]
        [Column("person_contact_id")]
        public int Id { get; set; }

        [Column("person_id")]
        [ForeignKey(nameof(Person))]
        public int PersonId { get; set; }

        [Column("contact_type_id")]
        [ForeignKey(nameof(Type))]
        public int ContactTypeId { get; set; }

        [Column("contact_value")]
        public string Value { get; set; }

        public Person Person { get; set; }

        public ContactType Type { get; set; }
    }
}
