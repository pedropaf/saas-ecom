@model $rootnamespace$.Controllers.ChangeSubscriptionViewModel

@{
    ViewBag.Title = "ChangeSubscription";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h3><i class="fa fa-refresh"></i> Change subscription plan</h3>
<div class="row">
    <div class="container">
        <div class="change-subscription">
            @using (Html.BeginForm("ChangeSubscription", "SaasEcom", FormMethod.Post, new { @class = "form-horizontal", role = "form" }))
            {
                @Html.AntiForgeryToken()
                @Html.ValidationSummary("", new { @class = "text-danger" })

                <p>The payment for the change will be pro-rated. Don't worry about over paying.</p>
                <ul class="list-unstyled">
                    @foreach (var p in Model.SubscriptionPlans)
                    {
                        var check = p.FriendlyId == Model.CurrentSubscription ? "checked" : "";
                        <li>@Html.RadioButtonFor(m => m.NewPlan, p.FriendlyId, new { @Checked = check }) @p.Name <small>(@p.CurrencyDetails.CurrencySymbol@string.Format("{0:F2}", p.Price) / @p.Interval)</small></li>
                    }
                </ul>
                <input type="submit" class="btn btn-primary" value="Submit" id="submit" />
            }
        </div>
    </div>
</div>
