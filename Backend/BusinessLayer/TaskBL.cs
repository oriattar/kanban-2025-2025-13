using System;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class TaskBL
    {
        private TaskDTO dto;

        public TaskBL(TaskDTO task)
        {
            dto = task;
        }

        public TaskDTO ToDTO()
        {
            return dto;
        }

        public string Title
        {
            get => dto.Title;
            set
            {
                if (dto.IdColumn == "done")
                    throw new InvalidOperationException("Cannot change task in 'done' column");
                if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                    throw new ArgumentException("Title must be 1-50 characters long");
                dto.Title = value;
            }
        }

        public string Description
        {
            get => dto.Description;
            set
            {
                if (dto.IdColumn == "done")
                    throw new InvalidOperationException("Cannot change task in 'done' column");
                if (value.Length > 300)
                    throw new ArgumentException("Description too long");
                dto.Description = value;
            }
        }

        public DateTime DueDate
        {
            get => dto.DueTime;
            set
            {
                if (dto.IdColumn == "done")
                    throw new InvalidOperationException("Cannot change task in 'done' column");
                if (value < dto.CreationTime)
                    throw new ArgumentException("Due date cannot be before creation time");
                dto.DueTime = value;
            }
        }

        public void MoveTo(string newColumn, string user)
        {
            if (dto.IdColumn == "done")
                throw new InvalidOperationException("Task is already in 'done'");
            if (user != dto.Assignee)
                throw new UnauthorizedAccessException("Only assignee can move the task");
            if (newColumn == "backlog")
                throw new InvalidOperationException("Cannot move task backward");
            dto.IdColumn = newColumn;
        }
    }
}