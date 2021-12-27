using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class QueryDto : BaseDto
    {
    }

    // Removed in 2.1
    //public class QueryDtoProxy : IQueryDtoProxy
    //{
    //    private IProjectController projectController_;
    //    private Guid queryId_;

    //    public QueryDtoProxy(IProjectController projectController, Guid queryId)
    //    {
    //        projectController_ = projectController;
    //        queryId_ = queryId;
    //    }

    //    [CategoryAttribute("(ID Settings)"),
    //     DescriptionAttribute("Unique Name String"),
    //     ParenthesizePropertyName(true)]
    //    public string Name
    //    {
    //        get
    //        {
    //            QueryDto query = projectController_.GetQuery(queryId_);

    //            return query.Name;
    //        }
    //        set
    //        {
    //            try
    //            {
    //                projectController_.SetQueryName(queryId_, value);
    //            }
    //            catch (InvalidNameException ex)
    //            {
    //                MessageBox.Show(ex.Message);
    //            }
    //        }
    //    }

    //    [BrowsableAttribute(false)]
    //    public Guid Id
    //    {
    //        get { return queryId_; }
    //    }

    //    [BrowsableAttribute(false)]
    //    public Guid OwnerId
    //    {
    //        get
    //        {
    //            QueryDto query = projectController_.GetQuery(queryId_);

    //            return query.OwnerId;
    //        }
    //    }
    //}
}
