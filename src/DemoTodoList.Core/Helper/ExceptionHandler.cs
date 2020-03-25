using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoTodoList.Core.Helper
{
  public  class ExceptionHandler
    {
        public static void ProcessException(Exception ex, IUserDialogs userDialogs)
        {

            if (ex is ArgumentException  appException && userDialogs != null)
            {
                userDialogs.Alert(new AlertConfig
                {
                    Message = ex.Message,
                    Title = "Validation",
                    OkText = "ok"
                }); ; ;
            }
            else
            {

                if (userDialogs != null)
                {
                    userDialogs.Alert(new AlertConfig
                    {
                        Message = "oops, We will check that error",                       
                        OkText = "Ok"
                    });
                }

            }
        }
    }
}
