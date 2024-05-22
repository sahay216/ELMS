$(document).ready(function () {
    $('#companyRegistrationForm').submit(function (event) {
        event.preventDefault(); // Prevent form submission
        var isError = false;
        $('.invalid-feedback').hide().text(''); // Reset error messages

        // Validation for Company Name
        var companyName = $('#CompanyName').val();
        if (!companyName) {
            $('#CompanyName').next('.invalid-feedback').text('Please enter company name').show();
            isError = true;
        }

        // Validation for Date of Establishment
        var dateOfEstablishment = $('#DateOfEstablishment').val();
        if (!dateOfEstablishment) {
            $('#DateOfEstablishment').next('.invalid-feedback').text('Please select date of establishment').show();
            isError = true;
        }

        // Validation for Industry
        var industry = $('#Industry').val();
        if (!industry) {
            $('#Industry').next('.invalid-feedback').text('Please enter industry').show();
            isError = true;
        }

        // Validation for Location
        var location = $('#Location').val();
        if (!location) {
            $('#Location').next('.invalid-feedback').text('Please enter location').show();
            isError = true;
        }

        // Validation for Country
        var country = $('#Country').val();
        if (!country) {
            $('#Country').next('.invalid-feedback').text('Please enter country').show();
            isError = true;
        }

        // Validation for Website
        var website = $('#Website').val();
        if (!website) {
            $('#Website').next('.invalid-feedback').text('Please enter website').show();
            isError = true;
        }

        // Validation for Email
        var email = $('#Email').val();
        if (!email) {
            $('#Email').next('.invalid-feedback').text('Please enter email').show();
            isError = true;
        }

        // Validation for Domain Name
        var domainName = $('#DomainName').val();
        if (!domainName) {
            $('#DomainName').next('.invalid-feedback').text('Please enter domain name').show();
            isError = true;
        }

        // Validation for Password
        var password = $('#Password').val();
        if (!password) {
            $('#Password').next('.invalid-feedback').text('Please enter password').show();
            isError = true;
        }

        // Validation for Phone
        var phone = $('#Phone').val();
        if (!phone) {
            $('#Phone').next('.invalid-feedback').text('Please enter phone number').show();
            isError = true;
        }

        // Validation for Address
        var address = $('#Address').val();
        if (!address) {
            $('#Address').next('.invalid-feedback').text('Please enter address').show();
            isError = true;
        }

        // Additional validation for other fields as needed...

        if (isError) {
            return;
        }

        // If all validations pass, submit the form
        this.submit();
    });
});