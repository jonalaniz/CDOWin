using CDO.Core.DTOs.SAs;
using CDO.Core.ErrorHandling;
using CDO.Core.WordInterop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CDOWin.Composers;

public sealed class ServiceAuthorizationComposer(SADetail sa) {
    private readonly SADetail _sa = sa;
    private readonly ITemplateProvider _templateProvider = new TemplateProvider();

    public Task<Result> Compose() {
        var tcs = new TaskCompletionSource<Result>();

        if (_templateProvider.GetTemplate("Invoice.dotx") is not string path) {
            tcs.SetResult(Result.Fail(new AppError(ErrorKind.Unknown, "SADetail Template not found.")));
            return tcs.Task;
        }

        var thread = new Thread(() => {
            try {
                var wordService = new WordInteropService();
                wordService.ExportServiceAuthorization(path, _sa);

                tcs.SetResult(Result.Success());
            } catch (Exception ex) {
                tcs.SetResult(Result.Fail(new AppError(ErrorKind.Unknown, "Failed to export Service Authorization", Exception: ex)));
            }
        });

        thread.SetApartmentState(ApartmentState.STA); // MUST do before Start
        thread.Start();

        return tcs.Task;
    }
}
