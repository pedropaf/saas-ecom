@model $rootnamespace$.Controllers.CancelSubscriptionViewModel
@{
    ViewBag.Title = "Cancel Subscription";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h3>Cancel Subscription</h3>
@using (Html.BeginForm())
{
    @Html.AntiForgeryToken()
	@Html.HiddenFor(m => m.Id)
    <div class="form-horizontal">
        @Html.ValidationSummary(true, "", new { @class = "text-danger" })
        <div class="form-group">
            @Html.LabelFor(model => model.Reason, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.Reason, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.Reason, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Cancel Subscription" class="btn btn-default" />
            </div>
        </div>
    </div>
}
