using CDO.Core.ErrorHandling;
using CDO.Core.Models;
using CDO.Core.WordInterop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CDOWin.Composers;

public sealed class ServiceAuthorizationComposer(ServiceAuthorization sa) {
    private readonly ServiceAuthorization _sa = sa;
    private readonly ITemplateProvider _templateProvider = new TemplateProvider();

    public Task<Result<string>> Compose() {
        var tcs = new TaskCompletionSource<Result<string>>();

        if (_templateProvider.GetTemplate("Invoice.dotx") is not string path) {
            tcs.SetResult(Result<string>.Fail(new AppError(ErrorKind.Unknown, "Invoice Template not found.")));
            return tcs.Task;
        }


        var thread = new System.Threading.Thread(() => {
            try {
                var wordService = new WordInteropService();
                wordService.ExportServiceAuthorization(path, _sa);

                tcs.SetResult(Result<string>.Success("success"));
            } catch (Exception ex) {
                tcs.SetResult(Result<string>.Fail(new AppError(ErrorKind.Unknown, "Failed to export Service Authorization", Exception: ex)));
            }
        });

        thread.SetApartmentState(ApartmentState.STA); // MUST do before Start
        thread.Start();

        return tcs.Task;
    }
}
