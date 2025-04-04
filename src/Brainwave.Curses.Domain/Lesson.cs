using Brainwave.Core.DomainObjects;

namespace Brainwave.Curses.Domain
{
    public class Lesson : Entity
    {
        public Lesson(Guid curseId, string title, string content, string material)
        {
            Title = title;
            Content = content;
            Material = material;
        }

        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Material { get; private set; }


        public Guid CurseId { get; private set; }

        // EF Rel.
        public Curse Curse { get; set; }

        internal void AssociateCurse(Guid curseId)
        {
            CurseId = curseId;
        }

        public override bool IsValid()
        {
            return string.IsNullOrWhiteSpace(Title) == false &&
                   string.IsNullOrWhiteSpace(Content) == false;
        }
    }
}
