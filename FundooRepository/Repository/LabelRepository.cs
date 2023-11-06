using FundooModel.Label;
using FundooModel.Notes;
using FundooRepository.Context;
using FundooRepository.IRepository;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace FundooRepository.Repository
{
    public class LabelRepository : ILabelRepository
    {
        public readonly UserDbContext context;
        Nlog nlog = new Nlog();
        public LabelRepository(UserDbContext context)
        {
            this.context = context;
        }
        public Task<int> AddLabel(Label label)
        {
            try
            {
                this.context.Labels.Add(label);
                var result = this.context.SaveChangesAsync();
                return result;
            }catch (Exception ex)
            {
                return null;
            }
        }
        public Label UpdateLabel(Label label)
        {
            try
            {
                var result = this.context.Labels.Where(x => x.LabelId == label.LabelId).FirstOrDefault();
                result.LabelName = label.LabelName;
                this.context.Labels.Update(result);
                var data = this.context.SaveChanges();
                if (data != 0)
                    return result;
                return null;
            }catch(Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<Label> GetAllLabels(int userId)
        {
            try
            {
                var result = this.context.Labels.Where(x => x.Id == userId).AsEnumerable();
                if (result != null)
                    return result;
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool DeleteLabel(int LabelId)
        {
            try
            {
                var result = this.context.Labels.Where(x => x.LabelId == LabelId).FirstOrDefault();
                if (result != null)
                {

                    this.context.Labels.Remove(result);
                    var deleteLabel = this.context.SaveChanges();
                    if (deleteLabel != 0)
                        return true;
                }
                return false;
            }catch (Exception ex)
            {
                return false;
            }
        }
        public IEnumerable<Label> GetAllLabelNotes(int userId)
        {
            List<Label> list = new List<Label>();
            var result = this.context.Notes.Where(x => x.Id.Equals(userId)).Join(this.context.Labels,
                Note => Note.Id,
                Label => Label.LabelId,
                (Note, Label) => new Label
                {
                    NoteId = Note.Id,
                    LabelId = Label.LabelId,
                    LabelName = Label.LabelName,
                });
            foreach (var data in result)
            {
                list.Add(data);
            }
            nlog.LogInfo("Retrieved Sucessful");

            return list;
        }

    }
}
