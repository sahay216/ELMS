$(document).ready(function () {
        
        $('#Signupform').submit(function (event) {

            event.preventDefault(); // Always prevent the default first
            
            var isError = false;
            $('.invalid-feedback').hide().text(''); // Reset error messages


            var firstName = $('#FirstName').val();
            if (!/^[a-zA-Z'-]+$/.test(firstName) || firstName==null) {
                $('#FirstName').next('.invalid-feedback').text('Name can only have the English alphabet, hyphens, and apostrophes').show();
                console.log(firstName)
                console.log(firstName)
                console.log("form validating....2");
                isError = true; // Set flag to true if there's an error
            }

            var lastName = $('#LastName').val();
            if (!/^[a-zA-Z'-]+$/.test(lastName)) {
                $('#LastName').next('.invalid-feedback').text('Name can only have the English alphabet, hyphens, and apostrophes').show();
                isError = true; // Set flag to true if there's an error
            }

            var phoneNumber = $('#PhoneNumber').val();
            if (!/^\d{10}$/.test(phoneNumber)) {
                $('#PhoneNumber').next('.invalid-feedback').text('Phone number must be exactly 10 digits.').show();
                isError = true;
            }


            var password = $('#Password').val();
            if (!/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/.test(password)) {
                $('#Password').next('.invalid-feedback').text('Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character').show();
                isError = true; // Set flag to true if there's an error
            }

            var confirmPassword = $('#ConfirmPassword').val();
            if (confirmPassword !== password) {
                $('#ConfirmPassword').next('.invalid-feedback').text('Passwords do not match').show();
                isError = true; // Set flag to true if there's an error

            }

            var dateOfBirth = $('#DateofBirth').val();
            var today = new Date();
            var birthDate = new Date(dateOfBirth);
            var age = today.getFullYear() - birthDate.getFullYear();

            // Check if the calculated age falls within the desired range
            if (age < 10 || age > 100 || dateOfBirth==="") {
                $('#DateOfBirth').next('.invalid-feedback').text('Age must be between 10 and 100 years').show();
                isError = true; // Set flag to true if there's an error
            }


            if (isError) {
                console.log("form validating....2");
                return;
            }

            this.submit(); // Prevent form submission if there are errors

        });

        $.ajax({
            url: 'CompanyAdmin/EmployeeRegistration',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.errors.length > 0) {
                    var errorMessage = "Errors:\n";
                    response.errors.forEach(function (error) {
                        errorMessage += "- " + error + "\n";
                    });
                    alert(errorMessage);
                }
            }
        });

    });