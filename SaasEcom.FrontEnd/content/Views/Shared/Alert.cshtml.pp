
@model $rootnamespace$.Views.SaasEcom.ViewModels.FlashViewModel

<div class="alert alert-@Model.CSS alert-dismissable">
    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
    @switch (Model.CSS)
    {
        case ("success"):
            @:<i class="fa fa-check"></i>
            break;
        case ("info"):
        @:<i class="fa fa-info"></i>
            break;
        case ("warning"):
        @:<i class="fa fa-exclamation"></i>
            break;
        case ("danger"):
        @:<i class="fa fa-exclamation"></i>
            break;
    }
    <strong> @Model.Message</strong>
</div>