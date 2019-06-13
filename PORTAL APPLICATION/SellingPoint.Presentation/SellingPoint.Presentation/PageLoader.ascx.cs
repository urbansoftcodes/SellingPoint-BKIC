using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Presentation
{
    public partial class PageLoader : System.Web.UI.UserControl
    {
        // private bool showLoading;

        public bool ShowLoading
        {
            set
            {
                if (value == true)
                {
                    this.MPE.Show();
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "showPageLoader", "showPageLoader();", true);
                }
                else
                {
                    this.MPE.Hide();
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "hidePageLoader", "hidePageLoader();", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}