@model SaasEcom.Core.Models.BillingAddress

@{
    ViewBag.Title = "Billing Address";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h3>Billing Address</h3>
<hr />
<div class="col-md-12 content">
    <div class="col-lg-10 col-md-12">
        @using (Html.BeginForm("BillingAddress", "Billing", FormMethod.Post, new { @class = "form-horizontal", role = "form" }))
        {
            @Html.AntiForgeryToken()

            <div class="form-horizontal">
                @Html.ValidationSummary(true, "", new { @class = "text-danger" })
                <div class="form-group">
                    @Html.LabelFor(model => model.Name, htmlAttributes: new { @class = "control-label col-md-2" })
                    <div class="col-md-10">
                        @Html.EditorFor(model => model.Name, new { htmlAttributes = new { @class = "form-control", placeholder = "Your Name or Company Name" } })
                        @Html.ValidationMessageFor(model => model.Name, "", new { @class = "text-danger" })
                    </div>
                </div>

                <div class="form-group">
                    @Html.LabelFor(model => model.AddressLine1, htmlAttributes: new { @class = "control-label col-md-2" })
                    <div class="col-md-10">
                        @Html.EditorFor(model => model.AddressLine1, new { htmlAttributes = new { @class = "form-control", placeholder = "Address Line 1" } })
                        @Html.ValidationMessageFor(model => model.AddressLine1, "", new { @class = "text-danger" })
                    </div>
                </div>

                <div class="form-group">
                    @Html.LabelFor(model => model.AddressLine2, htmlAttributes: new { @class = "control-label col-md-2" })
                    <div class="col-md-10">
                        @Html.EditorFor(model => model.AddressLine2, new { htmlAttributes = new { @class = "form-control", placeholder = "Address Line 2" } })
                        @Html.ValidationMessageFor(model => model.AddressLine2, "", new { @class = "text-danger" })
                    </div>
                </div>

                <div class="form-group">
                    @Html.LabelFor(model => model.City, htmlAttributes: new { @class = "control-label col-md-2" })
                    <div class="col-md-10">
                        @Html.EditorFor(model => model.City, new { htmlAttributes = new { @class = "form-control", placeholder = "City" } })
                        @Html.ValidationMessageFor(model => model.City, "", new { @class = "text-danger" })
                    </div>
                </div>

                <div class="form-group">
                    @Html.LabelFor(model => model.State, htmlAttributes: new { @class = "control-label col-md-2" })
                    <div class="col-md-10">
                        @Html.EditorFor(model => model.State, new { htmlAttributes = new { @class = "form-control", placeholder = "State" } })
                        @Html.ValidationMessageFor(model => model.State, "", new { @class = "text-danger" })
                    </div>
                </div>

                <div class="form-group">
                    @Html.LabelFor(model => model.ZipCode, htmlAttributes: new { @class = "control-label col-md-2" })
                    <div class="col-md-10">
                        @Html.EditorFor(model => model.ZipCode, new { htmlAttributes = new { @class = "form-control", placeholder = "Zip Code" } })
                        @Html.ValidationMessageFor(model => model.ZipCode, "", new { @class = "text-danger" })
                    </div>
                </div>

                <div class="form-group">
                    @Html.LabelFor(model => model.Country, htmlAttributes: new { @class = "control-label col-md-2" })
                    <div class="col-md-10">
                        @Html.EditorFor(model => model.Country, new { htmlAttributes = new { @class = "form-control", placeholder = "Country" } })
                        @Html.ValidationMessageFor(model => model.Country, "", new { @class = "text-danger" })
                    </div>
                </div>

                <div class="form-group">
                    @Html.LabelFor(model => model.Vat, htmlAttributes: new { @class = "control-label col-md-2" })
                    <div class="col-md-10">
                        @Html.EditorFor(model => model.Vat, new { htmlAttributes = new { @class = "form-control", placeholder = "VAT Number" } })
                        @Html.ValidationMessageFor(model => model.Vat, "", new { @class = "text-danger" })
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <button type="submit" class="btn green-meadow btn-lg btn-block">Save Billing Address</button>
                    </div>
                </div>
            </div>
        }
    </div>
</div>

@section Scripts {
    @Scripts.Render("~/bundles/jqueryval")
}