var app = angular.module('MyApp', ['ngTable', 'angucomplete-alt', 'moment-picker', 'cp.ngConfirm', 'ngFileUpload', 'ngMessages', 'directive.contextMenu']);
app.controller('MyCtrl', function ($rootScope, $scope, $http, NgTableParams, $ngConfirm, $q, Upload) {
    /*start_workin*/

    //----------------------- popup code for The Sample Aprove Dashboard---------------------

    // jQuery


    //--------------------------------------Dashboard Page------------------------

    $scope.alert = function (title, msg, btnclss, typeclr) {
        $ngConfirm({
            title: title,
            content: msg,
            type: typeclr,
            typeAnimated: true,
            buttons: {
                close: {
                    text: 'OK',
                    btnClass: btnclss,
                    action: function () {
                        if (title === 'Success !!')
                            location.reload();
                    }
                }
            }
        });
    };
    $scope.INIT = () => {

        angular.element('#loading').hide();

        $http.get("/Home/Administrator2").then(function (res) {
            if (res != null) {
                //$scope.Alltowelinfo1 = res.data.abcd;

                $scope.usersTable_Sm = new NgTableParams({}, { dataset: res.data.abcd });
                //$scope.alert('', "Find Success", 'btn-success', 'green', 'close');
            }
            else {
                $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
            }
        });

        $http.get("/Home/Costing_List2").then(function (res) {

            if (res != null) {
                $scope.Alltowelinfo1 = res.data.abc;
                $scope.usersTable_CS = new NgTableParams({}, { dataset: res.data.abc });

                //$scope.alert('', "Find Success", 'btn-success', 'green', 'close');
            }
            if (res == null) {
                $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
            }
        });

        $scope.Sm_View = true;
        //$scope.getStatusColor(status);
        disableInputBoxes();
    };

    //////////Side Nave Bare Highlighted when we Enter in the Name bar Menu
    var currentUrl = window.location.href;

    // Loop through each menu item and compare its href with the current URL
    $('.navigation-main .nav-item').each(function () {
        var menuItemUrl = $(this).find('a').attr('href');
        if (currentUrl.includes(menuItemUrl)) {
            $(this).addClass('active'); // Add a class to highlight the active menu item
        }
    })

    $scope.downloadPDF = function (pdfUrl) {
        // Create a temporary anchor element
        var link = document.createElement('a');
        link.href = pdfUrl;
        link.target = '_blank';
        link.download = 'download.pdf'; // Set the desired filename for the downloaded file

        // Append the anchor element to the document body
        document.body.appendChild(link);

        // Trigger a click event on the anchor element
        link.click();

        // Remove the anchor element from the document body
        document.body.removeChild(link);
    }


    function disableInputBoxes() {
        var inputBoxes = document.querySelectorAll('#SamplteDivId input');
        inputBoxes.forEach(function (input) {
            input.disabled = true;
        });
    }


    $scope.getStatusColor = function (status) {
        if (status === 'Pending') {
            return 'blue';
        } else if (status === 'Approved') {
            return 'green';
        } else if (status === 'Reject') {
            return 'red';
        } else {
            return 'black'; // default color if status doesn't match any condition
        }
    };

    $scope.Approved = function (DocEntry) {
        $http.get('/Home/Administrator2_Approv?DocEntry=' + DocEntry).then(function (res) {
            if (res.data = "YES") {
                $scope.alert('Alert !!', "Aprovel Success at DocEntry = '" + DocEntry + "' ", 'btn-success', 'green');
                window.addEventListener('click', function () {
                    location.reload();
                });
            }
        });
    }

    $scope.Approved_Costing = function (x) {
        angular.element('#loading').show();
        var params = {
            DocEntry: x.docentry,
            CustomerCode: x.CustomerCode
        };
        $http.get('/Home/Costing_List_App', { params: params }).then(function (res) {
            angular.element('#loading').hide();
            if (res.data == "SAPItmCreated") {
                $scope.alert('Alert !!', "Aprovel Success", 'btn-success', 'green');
                window.addEventListener('click', function () {
                    location.reload();
                });
            }
            else if (res.data != "SAPItmCreated") {

                $scope.alert('Alert !!', "Aprovel Not Success = '" + res.data + "' ", 'btn-Danger', 'red');
            }
            else {
                $scope.alert('Alert !!', "Aprovel Not Success = '" + res.data + "' ", 'btn-Danger', 'red');
            }
        });
    }
    $scope.Reject_Costing = function (DocEntry) {
        $http.get('/Home/Costing_List_Reject?DocEntry=' + DocEntry).then(function (res) {
            if (res.data = "YES") {
                $scope.alert('Alert !!', "Rejected Costing at DocEntry = '" + DocEntry + "' ", 'btn-Danger', 'Red');
                window.addEventListener('click', function () {
                    location.reload();
                });
            }
        });
    }
    $scope.Reject_Semple = function (DocEntry) {
        var promptResult = prompt('Alert !!', "Give Remarks for this Rejection at DocEntry = '" + DocEntry + "' ", 'btn-Danger', 'Red');
        if (promptResult !== null) {
            // User clicked OK
            $scope.Remarks_sm = promptResult;
            $http.get('/Home/Administrator2_Reject?DocEntry=' + DocEntry + '&remarks=' + $scope.Remarks_sm).then(function (res) {
                if (res.data = "YES") {
                    $scope.alert('Alert !!', "Rejection Success at DocEntry = '" + DocEntry + "' ", 'btn-Danger', 'Red');
                    window.addEventListener('click', function () {
                        location.reload();
                    });
                }
            });
        } else {
            $scope.alert('Alert !!', "Rejection Cancle at DocEntry = '" + DocEntry + "' ", 'btn-Danger', 'Red');
        }
    }

    $scope.Sample_View = function (DocEntry) {
        $scope.Sm_View = false;

        $http.get('/Home/Administrator2_View?DocEntry=' + DocEntry).then(function (res) {
            if (res.data.Samplte_view.length != 0) {

                $scope.Sample = res.data.Samplte_view[0];
                $scope.Sample_R = res.data.Samplte_R;

                $scope.DocEntry_View = DocEntry;
            }
        });
    }


    $scope.downloadFile = function (filePath) {
        // Splitting the filePath into path and fileName
        var pathAndFileName = filePath.split("\\");
        var path = pathAndFileName.slice(0, -1).join("\\"); // Join all parts except the last one
        var fileName = pathAndFileName.slice(-1)[0]; // Get the last part as fileName

        // Calling the backend function
        window.location.href = '/Home/DownloadFile?filePath=' + encodeURIComponent(path) + '&fileName=' + encodeURIComponent(fileName);
    };

    function handleResponse(response) {
        if (response === "file_not_support") {
            window.alert("File not supported");
        } else {
            // Handle other responses or actions
        }
    }

    $scope.Search_Data = function () {
        try {
            $http.get("/Home/FindCustomer?SessiouserDep=" + SessiouserUserBranch).then(function (res) {
                if (res != null) {
                    $scope.Alltowelinfo1 = res.data.abc;
                    $scope.usersTable5 = new NgTableParams({}, { dataset: res.data.abc });
                    $scope.AuditDepshow = false;
                    //$scope.alert('', "Find Success", 'btn-success', 'green', 'close');
                }
                if (res == null) {
                    $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
                }
            });

        }
        catch (ex) {

        }

    }
    $scope.PrintRepaort = true;
    //Repot Print
    $scope.PrintRep = (elem) => {
        $scope.PrintRepaort = false;

        var printElement = document.getElementById(elem);

        if (printElement) {
            var printContents = printElement.innerHTML;
            var originalContents = document.body.innerHTML;

            // Open a new window for printing
            var printWindow = window.open('', '_blank');

            // Write the HTML content to the new window
            printWindow.document.write('<html><head><title>' + document.title + '</title></head><body>');
            printWindow.document.write(printContents);
            printWindow.document.write('</body></html>');

            // Print and close the new window
            printWindow.document.close();
            printWindow.print();
            printWindow.onafterprint = function () {
                printWindow.close();
            };

            // Restore the original content to the main window
            document.body.innerHTML = originalContents;
            location.reload();
        } else {
            console.error("Element with ID '" + elem + "' not found.");
        }
    };
    //dlt input data
});

app.controller('MainPage', function ($rootScope, $scope, $http, NgTableParams, $ngConfirm, $q, Upload) {
    $scope.TowelInit = function () {
        try {
            angular.element('#loading').show();
            $scope.BathSheetArry = [];
            $scope.BathTowelArry = [];
            $scope.HandTowelArry = [];
            $scope.WashClotharry = [];
            $scope.WashGlovearry = [];
            $scope.Customerarry = [];
            $scope.Alltowelinfo = [];
            $scope.Alltowelinfo1 = {};
            $scope.Alltowelinfo2 = {};
            $scope.Revise_val = true;
            $scope.SAL_UDT_BTN = true;
            $scope.Edtbydesign = false;
            $scope.T = {};
            $scope.Alltowelinfo1.BathTowelArry = [];
            $scope.Alltowelinfo1.BathSheetArry = [];
            $scope.Alltowelinfo1.HandTowelArry = [];
            $scope.Alltowelinfo1.WashClotharry = [];
            $scope.Alltowelinfo1.WashGlovearry = [];
            $scope.Alltowelinfo1.General_Inputs = [];
            $scope.Alltowelinfo1.SAL_Detail = [];
            $scope.Alltowelinfo1.Trims = [];
            $scope.Production_Process_Detail = [];
            $scope.DyeschemItemArray = [];
            $scope.PckingTItemArray = [];
            $scope.StechingthreadItemArray = [];
            $scope.BordenPollyshtItemArray = [];
            $scope.PDQItemArray = [];
            $scope.Pile_Yarn_val = ["Single", "Double"]
            $scope.Terry_Towels = {};
            $scope.BtnName = "Submit";
            $scope.RenewvalBtn = true;
            disableAccUser();
            $http.get('/api/WebAPI/TeryTowelInit').then(function (response) {
                $scope.Mat = response.data.Material_name;
                $scope.Currency_dtl = response.data.Curry_dtl;

                $scope.Weaving_name = response.data.Name;
                //$scope.Alltowelinfo1.Costing_Num = response.data.lastentrydata[0].DocEntry;
                $scope.OCRD = response.data.OCRD;
                $scope.OCRD = response.data.OCRD;
                $scope.SaleEmployee = response.data.SaleEmployee;
                $scope.INPUTDATA = response.data.INPUTDATA;
                $scope.Particulars = response.data.Particulars;
                $scope.infoType = response.data.Type;
                $scope.Percentage_val = response.data.Percentage;
                $scope.infocountdata = response.data.countdata;
                $scope.info_card_cmbd_OE = response.data.card_cmbd_OE;
                $scope.Slab_Name = response.data.Slab_name;
                $scope.Slab_Value = response.data.Slab_Value;
                $scope.SD = response.data.SD;
                $scope.DOBBY = response.data.DOBBY;
                $scope.COLOR = response.data.COLOR;
                $scope.DYED = response.data.DYED;
                $scope.SHEARING = response.data.SHEARING;
                $scope.UNITS = response.data.UNITS;
                $scope.POLYESTER = response.data.POLYESTER;
                $scope.Production_Process = response.data.Production_Process;
                $scope.Total_Costing = response.data.counts[0];
                $scope.Total_Ap_Costing = response.data.counts[1];
                $scope.Total_Rj_Costing = response.data.counts[2];
                $scope.Total_Pn_Costing = response.data.counts[3];
                $scope.Itm = response.data.Itm;
                $scope.AuditDep = [];
                //$scope.AuditDep = response.data.AuditDep;
                angular.element('#loading').hide();
            }).catch(function (response) {
                $scope.alert('Alert !!', response.data, 'btn-danger', 'red');
                console.log(response.data);
            })

        }
        catch (ex) {
            $scope.HideLoader();
            $scope.alert('Alert !!', ex, 'btn-danger', 'red');
            console.log(ex);
        }
        $scope.Updatebtn = false;
        $scope.Addbtn = true;
        $scope.materiallist = true;
        $scope.Afterclickcosttype = false;
        $scope.Customer_Code_div = true;
        $scope.All_data_on_search = true;
        $scope.angodiv = true;
        $scope.namediv = false;
        $scope.Costing_num_at_Edit = true;
        $scope.Costing_num_at_Insert = false;
        $scope.date_on_edit = true;
        $scope.date_on_Insert = false;
        $scope.Hide_btn_ud_tinfo = true;
        $scope.Hide_btn_Dlt_tinfo = false;
        $scope.T_info_headings = true;
        $scope.Reviev_hide = true;
        $scope.Itemmodel = true;
        $scope.HideModal_Sales = true;
        $scope.HideModal_Sales_PSC = true;
        $scope.General_Inputs.Current_Exchange_Rate_val = 0;
        //$scope.Revise_val = false;
    };


    /////// Upload Towel Details With Excel File
    $scope.UploadFile = function (files) {
        $scope.$apply(function () { //I have used $scope.$apply because I will call this function from File input type control which is not supported 2 way binding  
            $scope.Message = "";
            $scope.SelectedFileForUpload = files[0];
            $scope.showexcel = false;
            $scope.punchsap = false;
        });
        $scope.ParseExcelDataAndSave();
    };

    $scope.ParseExcelDataAndSave = function () {
        var file = $scope.SelectedFileForUpload;
        if (file !== null) {
            let splitFileName = file.name.split(".")[1];
            if (splitFileName === 'xls' || splitFileName === 'xlsx') {
                if (file.size <= 1048576) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var data = e.target.result;
                        var workbook = XLSX.read(data, { type: 'binary' });
                        if (workbook.SheetNames.length == 1) {
                            var sheetName = workbook.SheetNames[0];
                            var excelData = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
                            $scope.All = excelData;
                            var expectedColumns = [
                                'Particulars',
                                'COLOR_VAL',
                                'Color_Shades',
                                'Design',
                                'Length',
                                'Width',
                                'GSMlbsdoz',
                                'PcWeight',
                                'DyWgt',
                                'Shearingloss',
                                'GryWgt',
                                'Qty',
                                'TotalKg',
                                'WaveLossper',
                                'planQty',
                                'planTotalKg',
                                'OrderplanTotalper',
                                'Pricepc',
                                'PriceKgs',
                                'TotalWaveLoss',
                            ];
                            var columnNames = Object.keys(excelData[0]);
                            var columnsMatch = expectedColumns.every(function (column) {
                                return columnNames.includes(column);
                            });
                            if (columnsMatch) {
                                if (excelData.length > 0) {
                                    $scope.TowelExcelArry = [];
                                    $scope.TowelExcelArry = excelData;
                                } else {
                                    alert();
                                    $scope.alert('Alert !!', "No data found", 'btn-danger', 'red');
                                }
                            } else {
                                $scope.alert('Alert !!', "Columns do not match with valid Excel Sheet !!", 'btn-danger', 'red');
                            }
                        }
                        else {
                            $scope.alert('Alert !!', "This Excel has More than one Sheet !!", 'btn-danger', 'red');
                        }

                    };
                    reader.onerror = function (ex) {
                        console.log(ex);
                    };
                    reader.readAsBinaryString(file);
                } else {
                    alertError();
                    $scope.alert('Alert !!', "File size should be less than or equal to 1 MB", 'btn-danger', 'red');
                }
            } else {
                $scope.alert('Alert !!', "Only Excel Files are Valid !!", 'btn-danger', 'red');
            }
        } else {
            $scope.alert('Alert !!', "Please select a file", 'btn-danger', 'red');
        }
    };
    $scope.PushExcelData = function () {
        var check = $scope.PushExcelDataDsgCheck()
        if (check) {
            if ($scope.TowelExcelArry != undefined) {
                if ($scope.TowelExcelArry.length > 0) {
                    $scope.Alltowelinfo1.BathSheetArry = [];
                    $scope.Alltowelinfo1.BathSheetArry = $scope.TowelExcelArry;
                    if ($scope.Alltowelinfo1.BathSheetArry.length !== 0) {
                        $scope.AddRow_Totals();
                    }
                } else {
                    alert();
                    $scope.alert('Alert !!', "No data found", 'btn-danger', 'red');
                }
            }
            else {
                $scope.alert('Alert !!', "Please Select the Excel File", 'btn-danger', 'red');
            }
        }
        else {
            $scope.alert('Alert !!', "Please fill all Design Section in Excel", 'btn-danger', 'red');
        }
    }
    $scope.PushExcelDataDsgCheck = function () {
        for (var i = 0; i < $scope.TowelExcelArry.length; i++) {
            if ($scope.TowelExcelArry[i].Design == undefined) {
                return false;
            }
        }
        return true;
    }

    ////////////////////Production Process///////////////////////////////////////////////////////////////////////////
    function disableAccUser() {
        if (SessiouserUserBranch == "Audit") {
            $scope.SessiouserUserBranch11 = SessiouserUserBranch;
            $scope.SessiouserDep = SessiouserDep;
            document.getElementById("HeaderDiv").disabled = true;
            var nodes = document.getElementById("HeaderDiv").getElementsByTagName('*');
            for (var i = 0; i < nodes.length; i++) {
                nodes[i].disabled = true;
            }

        }
    }
    function disableAccUser2() {
        if (SessiouserUserBranch == "EntirePage") {
            $scope.SessiouserUserBranch11 = SessiouserUserBranch;
            $scope.SessiouserDep = SessiouserDep;
            document.getElementById("EntirePage").disabled = true;
            var nodes = document.getElementById("HeaderDiv").getElementsByTagName('*');
            for (var i = 0; i < nodes.length; i++) {
                nodes[i].disabled = true;
            }

        }
    }


    $scope.Selected_process = function (i) {
        if (i.U_Process_Status == "No") {
            i.U_Process_Status = "Yes";
            $scope.Production_Process_Detail.push({
                Code: i.Code,
                U_Process_Name: i.U_Process_Name,
                U_Process_Code: i.U_Process_Code,
                U_Process_Status: i.U_Process_Status
            });
        }
        else {
            i.U_Process_Status = "No";
            $scope.Production_Process_Detail.splice(i.U_Process_Code, 1);

        }

    }
    $scope.GetMerCode = function () {
        for (var i = 0; i < $scope.SaleEmployee.length; i++) {
            if ($scope.SaleEmployee[i].SlpCode == $scope.merchandiserCode) {
                $scope.merchandiser = $scope.SaleEmployee[i].SlpName;

            }
        }
    }
    $scope.dlt_model_dtl = function (Tn) {
        for (var i = 0; i < $scope.Production_Process_Detail.length; i++) {
            if ($scope.Production_Process_Detail[i].TowelType === Tn) {
                $scope.Production_Process_Detail.splice(i, 1);
                i--;
            }
        }
    }


    $scope.openModal = function () {
        var modal = document.getElementById('myModal');
        var T_Name = document.getElementById('myModal').value;
        modal.style.display = 'block';

    }
    $scope.closeModal = function () {
        var modal = document.getElementById('myModal');
        modal.style.display = 'none';
        angular.forEach($scope.Production_Process, function (process) {
            // Reset checkbox state and U_Process_Status to 'No'
            process.selected = false;
            process.U_Process_Status = 'No';
        });

    }

    //////////////////////////////////////////////////// Price Order sale (POS)


    /////////////File Upload 
    $scope.Filearr = [];

    $scope.downloadFile = function (filePath) {
        // Splitting the filePath into path and fileName
        var pathAndFileName = filePath.split("\\");
        var path = pathAndFileName.slice(0, -1).join("\\"); // Join all parts except the last one
        var fileName = pathAndFileName.slice(-1)[0]; // Get the last part as fileName

        // Calling the backend function
        window.location.href = '/Home/DownloadFile?filePath=' + encodeURIComponent(path) + '&fileName=' + encodeURIComponent(fileName);
    };
    $scope.pthval = "";
    $scope.userId = "";
    $scope.userId = SessiouserId;
    $scope.userDepartment = SessiouserDep;
    $scope.Fileup = function () {
        var fileInput = document.getElementById('myFileInput');
        var file = fileInput.files[0];
        var pathcheck = fileInput.value.split("\\");
        $scope.pthval = file.name;
        //var attachedFile = fileLocation.concat("\\", file.name);
        if (!$scope.pthval.includes("'")) {
            $http.get('/Home/CheckFolder?attachedFile=' + encodeURIComponent($scope.pthval)).then(function (res) {
                if (res.data != "N") {
                    var fileLocation = res.data;
                    var attachedFile = fileLocation.concat("\\", file.name);
                    $scope.Filearr.push({ userDepartment: $scope.userDepartment, UserId: $scope.userId, file: attachedFile });
                    console.log($scope.Filearr);
                    fileInput.value = "";
                } else if (res.data == "N") {
                    $scope.alert('Alert !!', "Please select files exclusively from the directory E:\\Costing_Data and that particular department", 'btn-danger', 'red');

                    fileInput.value = "";
                }
                else {
                    $scope.alert('Alert !!', "File Not Found !!" + res.data, 'btn-danger', 'red');
                }

            });
        }
        else {
            $scope.alert('Alert !!', "Please  Rename this file and Remove the  ( ' ) !!", 'btn-danger', 'red');
        }

    };
    $scope.dltFileup = function (x) {
        $scope.Filearr.splice(x, 1);
    }

    $scope.uploadFile = function () {
        var file = $scope.File;
        Upload.upload({
            url: '/Home/Upload/',
            data: {
                files: $scope.SelectedFiles,
                ImageName: $scope.ImageName
            }
        }).then(function (response) {
            alert("File uploaded successfully");
        }, function (error) {
            // Error uploading file
        });
    }
    $scope.fileChanged = function () {
        var input = document.querySelector('input[type="file"]');
        if (input.files.length > 0) {
            $scope.Alltowelinfo1.ImageName = input.files[0].name; // Store the file name in the model
            $scope.$apply(); // Ensure the model updates in AngularJS
        }
    };

    //IntrestDepreciation calcutaion
    $scope.Afterclickcosttype_cheack = function () {
        if ($scope.Alltowelinfo1.Weaving_Type == "") {
            $scope.Afterclickcosttype = false;
        } else {
            $scope.Afterclickcosttype = true;
        }
        for (var i = 0; i < $scope.Weaving_name.length; i++) {
            if ($scope.Alltowelinfo1.Weaving_Type == $scope.Weaving_name[i].Name) {
                for (var j = 0; j < $scope.Mat.length; j++) {
                    if ($scope.Mat[j].Material_name == "Intrest & Depreciation") {
                        $scope.Mat[j].Material_Value = $scope.Weaving_name[i].Value;
                    }
                }
            }
        }

    }
    //////////////////////////////////////////////Total Costing Detail////////////////////////////////////////////
    $scope.Created_Costing = function () {
        $http.get("/Home/FindCustomer").then(function (res) {
            if (res != null) {
                $scope.Alltowelinfo1 = res.data.abc;
                $scope.usersTable = new NgTableParams({}, { dataset: res.data.abc });
            }
            if (res == null) {
                $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
            }
        });
    }
    //////////////////////////////////////////////Total Approved Costing Detail////////////////////////////////////////////
    $scope.FindApprovedCosting = function (i) {

        if (i == "Approved") {
            $http.get("/Home/FindApprovedCosting", { params: { Status: i } }).then(function (res) {
                if (res.data !== null) {
                    $scope.usersTable1 = new NgTableParams({}, { dataset: res.data.abc });
                } else {
                    $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
                }
            });
        }
        else if (i == "Reject") {
            $http.get("/Home/FindApprovedCosting", { params: { Status: i } }).then(function (res) {
                if (res.data !== null) {
                    $scope.usersTable2 = new NgTableParams({}, { dataset: res.data.abc });
                } else {
                    $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
                }
            });
        }
        else {
            $http.get("/Home/FindApprovedCosting", { params: { Status: i } }).then(function (res) {
                if (res.data !== null) {
                    $scope.usersTable3 = new NgTableParams({}, { dataset: res.data.abc });
                } else {
                    $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
                }
            });
        }
    }
    /// Set Minimum Value of Date Calender
    $(function () {
        var dtToday = new Date();

        var month = dtToday.getMonth() + 1;
        var day = dtToday.getDate();
        var year = dtToday.getFullYear();
        if (month < 10)
            month = '0' + month.toString();
        if (day < 10)
            day = '0' + day.toString();

        var minDate = year + '-' + month + '-' + day;

        $('#txtDate').attr('min', minDate);
    });

    $scope.CostingNum_Validation = function () {
        $http.get('/Home/CostingNum_Validation').then(function (ress) {
            for (var i = 0; i < ress.data.CostingNum.length; i++) {
                if ($scope.Alltowelinfo1.Costing_Num == ress.data.CostingNum[i].CostingNum) {

                    alert("This Coustomer Number Are Already Exist !!")
                    $scope.Alltowelinfo1.Costing_Num = "";
                    break;
                }
            }
        });
    }

    $scope.AfterSelectedItem = function (selected) {
        try {
            angular.element('#Items').val(selected.originalObject.ItemName);
            $scope.SD_VAL = selected.originalObject.ItemName;
            $scope.countdata = selected.originalObject.ItemCode;
            $scope.costdata = parseFloat(selected.originalObject.Priceperpc);
        }
        catch (ex) {
            console("Item not Found");
        }
    }
    $scope.AfterSelectedSOBP = function (selected) {
        try {
            angular.element('#CustomerName').val(selected.originalObject.CustomerName);
            $scope.CustomerName = selected.originalObject.CustomerName;
            $scope.CustomerCode = selected.originalObject.CustomerCode;
        }
        catch (ex) {
            console("Name not Found");
        }
    }
    $scope.AfterSelectedSOBP2 = function () {
        try {
            angular.element('#CustomerName2').val(selected.originalObject.CustomerName2);
            $scope.CustomerName = selected.originalObject.CustomerName;
            $scope.CustomerCode = selected.originalObject.CustomerCode;

            $http.get('/Home/Bill_to_Ship_to', { params: { Code: $scope.CustomerCode } }).then(function (ress) {
                if (ress.data.OCRD2 != null) {
                    $scope.B_S_Data = ress.data.OCRD;
                    $scope.B_S_Data2 = ress.data.OCRD2;
                    $scope.created_costing = ress.data.OCRD3;
                    if (ress.data.OCRD3.length == 0) {
                        $scope.alert('Alert !!', "No Costing for this '" + $scope.CustomerName + "'Customer '", 'btn-success', 'green');
                    }
                }
            });
        }
        catch (ex) {
            console("Name not Found");
        }
    }

    //////////////////// Validation For Uniq Sale Order Number ////////////////////
    $scope.UniqSaleOrder = function () {
        try {
            if ($scope.Alltowelinfo1.SaleNum != undefined && $scope.Alltowelinfo1.SaleNum != '') {
                $http.get('/Home/UniqSaleOrdercheck', { params: { Salenum: $scope.Alltowelinfo1.SaleNum } }).then(function (ress) {
                    if (ress.data == "Exist") {
                        $scope.alert('Alert !!', " This Sale's Order Number are already Exist !!", 'btn-danger', 'red');
                        $scope.Alltowelinfo1.SaleNum = '';
                    }
                });
            }
            else {
                $scope.alert('Alert !!', "Please Enter the Sale's Order No !!", 'btn-danger', 'red');
            }
        }
        catch (ex) {
            console("Error At Finding Sale's Order Value !!" + ex);
        }
    }
    $scope.Primary_Cash = function () {
        if ($scope.Alltowelinfo1.SalSrType == "Primary") {
            $scope.Alltowelinfo1.SaleNum = 0;
        }
    }
    //////////////////// Costing Detail for Sales 
    $scope.Costing_dtl = function () {
        $scope.DocEn = $scope.created_costing.filter(item => item.Costing_Num === $scope.Costing_Num)
            .map(item => item.DocEntry)
        var params = {
            Costing_num: $scope.Costing_Num,
            created_costing_DocEntry: $scope.DocEn[0]
        };

        $http.get('/Home/Costing_dtl', { params: params })
            .then(function (ress) {
                if (ress.data.length === 0) {
                    $scope.alert('Alert !!', "No Costing for this '" + $scope.CustomerName + "'Customer '", 'btn-success', 'green');
                }
                else {
                    $scope.Costing_Item = ress.data.Itm
                    $scope.Costing_num = ress.data.Itm2[0].CostingNum;
                    $scope.Costing_GT = ress.data.Itm2[0].Grand_Total;
                }
            });
    };
    //////////////////////////Sales Total Price
    $scope.Total_Sales_cal = function (item) {
        item.Total_Sales_p = parseFloat(($scope.Costing_GT * item.Total_for_Sales).toFixed(2));
    }

    $scope.dtlitem = function (i) {
        $scope.Costing_Item.splice(i, 1);
    }

    ///////Sale Order Punch//////////////
    $scope.Datevalidation = function () {

        try {

            $scope.currentDate = new Date();
            $scope.cdate1 = new Date($scope.Alltowelinfo1.cdate);

            const _d = new Date($scope.Alltowelinfo1.cdate);
            const day = _d.getDate().toString().padStart(2, '0');
            const month = (_d.getMonth() + 1).toString().padStart(2, '0');
            const year = _d.getFullYear().toString();
            $scope.formattedDate = `${day}/${month}/${year}`;

        }
        catch (ex) {
            alert("Date function not Responding!");
        }

    }

    $scope.Materialname_DB = function () {
        $http.get('/Home/Material_Name').then(function (ress) {
            $scope.Mat = res.data.Material_name;

        });
    }
    $scope.xy = auditUsertypeHide;
    $scope.Submite = function (i) {
        if ($scope.CustomerName != undefined && $scope.CustomerName != '') {
            if ($scope.Alltowelinfo1.cdate != undefined && $scope.Alltowelinfo1.cdate != null) {
                if ($scope.merchandiser != undefined && $scope.merchandiser != '') {
                    if ($scope.Alltowelinfo1.Dyed_Type != null) {
                        if ($scope.CRefNo != undefined && $scope.CRefNo != '') {
                            if ($scope.Alltowelinfo1.SalSrType == "Manual" && $scope.Alltowelinfo1.SaleNum != undefined && $scope.Alltowelinfo1.SaleNum != '' || $scope.Alltowelinfo1.SalSrType == "Primary") {
                                if ($scope.Production_Process_Detail.length != 0) {
                                    if ($scope.Filearr.length != 0) {
                                        if ($scope.Alltowelinfo1.Merch_Status != undefined && $scope.Alltowelinfo1.Merch_Status != '') {
                                            angular.element('#loading').show();
                                            if ($scope.BtnName == "Submit" || i == 'R') {
                                                $http.get('/Home/UniqSaleOrdercheck', { params: { Salenum: $scope.Alltowelinfo1.SaleNum } }).then(function (ress) {
                                                    if (ress.data == "NotExits") {
                                                        $scope.Alltowelinfo1.Customername = $scope.CustomerName;
                                                        $scope.Alltowelinfo1.CustomerCode = $scope.CustomerCode;
                                                        if ($scope.formattedDate == undefined) {
                                                            $scope.Alltowelinfo1.CostingDate = $scope.Alltowelinfo1.cdate;
                                                        }
                                                        else {
                                                            $scope.Alltowelinfo1.CostingDate = $scope.formattedDate;
                                                        }
                                                        $scope.Alltowelinfo1.DocEntry_val = parseInt($scope.DocEntry_val);
                                                        $scope.Alltowelinfo1.merchandiser = $scope.merchandiser;
                                                        $scope.Alltowelinfo1.merchandiserCode = $scope.merchandiserCode;
                                                        $scope.Alltowelinfo1.DYED_VAL = $scope.Alltowelinfo1.Dyed_Type;
                                                        $scope.Alltowelinfo1.Weaving_VAL = $scope.Alltowelinfo1.Weaving_Type
                                                        $scope.Alltowelinfo1.CostingNum = $scope.Alltowelinfo1.Costing_Num;
                                                        $scope.Alltowelinfo1.CRefNo = $scope.CRefNo;
                                                        $scope.Alltowelinfo1.Allvalues = [];
                                                        $scope.Alltowelinfo1.Allvalues = $scope.Alltowelinfo.concat(
                                                            $scope.Allvalues,
                                                        );
                                                        $scope.Alltowelinfo1.BathSheetArry = $scope.Alltowelinfo.concat(
                                                            $scope.Alltowelinfo1.BathSheetArry,
                                                        );
                                                        $scope.Alltowelinfo1.Filearr = $scope.Filearr;
                                                        $scope.Alltowelinfo1.Material_slab = $scope.Mat;
                                                        $scope.Alltowelinfo1.Production_Process = $scope.Production_Process_Detail;
                                                        $scope.Alltowelinfo1.General_Inputs = [];
                                                        $scope.Alltowelinfo1.General_Inputs[0] = $scope.General_Inputs;

                                                        $http.post('/Home/Insert', $scope.Alltowelinfo1, $scope.Revise_val).then(function (response) {
                                                            angular.element('#loading').hide();
                                                            if (response.data.Message == undefined) {
                                                                $scope.last_DocEntry = response.data.Data.lastentrydata[0].DocEntry;
                                                                $scope.SFG_Item_creare_function($scope.last_DocEntry);
                                                                //$scope.alert('Alert !!', "Submit Success at Costing Number = '" + $scope.last_costing_num + "'   And DocEntry = '" + $scope.last_DocEntry + "'", 'btn-success', 'green');
                                                                $scope.alert('Alert !!', "Submit Success at Costing Number = '" + response.data.Data.a + "' And DocEntry = '" + $scope.last_DocEntry + "'", 'btn-success', 'green');
                                                                window.addEventListener('click', function () {
                                                                    location.reload();
                                                                });
                                                            }
                                                            else {
                                                                $scope.alert('Alert !!', "Submition Failed" + response.data.Message.Message, 'btn-danger', 'red');
                                                            }
                                                        })
                                                    }
                                                    else {
                                                        $scope.alert('Alert !!', "The sales order number already exists !!", 'btn-danger', 'red');
                                                        angular.element('#loading').hide();
                                                    }
                                                });
                                            }
                                            //UpdateAll
                                            else {
                                                var ressss = $scope.containsUserIdCRR2($scope.userId);
                                                if (SessiouserUserBranch === 'Audit' && $scope.Alltowelinfo1.Audit_Status != undefined && $scope.Alltowelinfo1.Audit_Status != '' && ressss != false) {
                                                    $scope.UpdateAllSeprate();
                                                }
                                                else if (SessiouserUserBranch !== 'Audit') {
                                                    $scope.UpdateAllSeprate();
                                                }
                                                else {
                                                    angular.element('#loading').hide();
                                                    $scope.alert('Alert !!', "Please select Audit Action and Upload a file with currect User!!", 'btn-danger', 'red');
                                                }
                                            }
                                        }
                                        else {
                                            $scope.alert('Alert !!', "Please Choose select the Merchant Doc Statsu !!", 'btn-danger', 'red');
                                        }
                                    }
                                    else {
                                        $scope.alert('Alert !!', "Please Choose File !!", 'btn-danger', 'red');
                                    }
                                }
                                else {
                                    $scope.alert('Alert !!', "Please Select Process !!", 'btn-danger', 'red');
                                }
                            }
                            else {
                                $scope.alert('Alert !!', "Please Enter the Sale's Order Number !!", 'btn-danger', 'red');
                            }
                        }
                        else {
                            $scope.alert('Alert !!', "Please Enter the Customer Reference No !!", 'btn-danger', 'red');
                        }
                    }
                    else {
                        $scope.alert('Alert !!', "Please Select Dyed Type !!", 'btn-danger', 'red');
                    }
                }
                else {
                    $scope.alert('Alert !!', "Please select the Marchandiser !!", 'btn-danger', 'red');
                }
            }
            else {
                $scope.alert('Alert !!', "Please Enter Date of Costing !!", 'btn-danger', 'red');
            }
        }
        else {
            $scope.alert('Alert !!', "Please select Customer !!", 'btn-danger', 'red');
        }
    }
    $scope.UpdateAllSeprate = function () {
        $scope.Alltowelinfo2.DocEntry_val = parseInt($scope.DocEntry_val);
        $scope.Alltowelinfo2.Customername = $scope.CustomerName;
        $scope.Alltowelinfo2.CustomerCode = $scope.CustomerCode;
        $scope.Alltowelinfo2.CostingDate = $scope.formattedDate;
        $scope.Alltowelinfo2.merchandiser = $scope.merchandiser;
        $scope.Alltowelinfo2.merchandiserCode = $scope.merchandiserCode;
        $scope.Alltowelinfo2.SaleNum = parseInt($scope.Alltowelinfo1.SaleNum);
        $scope.Alltowelinfo2.DYED_VAL = $scope.Alltowelinfo1.Dyed_Type;
        $scope.Alltowelinfo2.Weaving_VAL = $scope.Alltowelinfo1.Weaving_Type;
        $scope.Alltowelinfo2.HRem = $scope.Alltowelinfo1.HRem;
        $scope.Alltowelinfo2.BathSheetArry = [];
        $scope.Alltowelinfo2.BathSheetArry = $scope.Alltowelinfo1.BathSheetArry;
        $scope.Alltowelinfo2.Allvalues = [];
        $scope.Alltowelinfo2.Allvalues = $scope.Allvalues;
        $scope.Alltowelinfo2.Material_slab = $scope.Mat;
        $scope.Alltowelinfo2.Filearr = $scope.Filearr;
        $scope.Alltowelinfo2.General_Inputs = [];
        $scope.Alltowelinfo2.General_Inputs[0] = $scope.General_Inputs
        $scope.Alltowelinfo2.CRefNo = $scope.CRefNo;
        $scope.Alltowelinfo2.Merch_Status = $scope.Alltowelinfo1.Merch_Status;
        $scope.Alltowelinfo2.Audit_Status = $scope.Alltowelinfo1.Audit_Status;
        $scope.Alltowelinfo2.DesignYarnInformation = $scope.DesignYarnInformation;
        $scope.Alltowelinfo2.Production_Process = $scope.Production_Process_Detail;
        $scope.Alltowelinfo2.Edtbydesign = $scope.Edtbydesign;
        if ($scope.Edtbydesign === true) {
            if ($scope.SalesPCSvali === 0 && $scope.SalesSETvali === 0) {
                angular.element('#loading').hide();
                $scope.alert('Alert !!', "Please select the sales as per the invoice !!", 'btn-danger', 'red');
                return;
            }
        }
        $http.post('/Home/UpdateAll', $scope.Alltowelinfo2).then(function (response) {
            if (response.data.Data == "OK") {
                $scope.alert('Alert !!', "Update Success", 'btn-success', 'green');
                window.addEventListener('click', function () {
                    location.reload();
                });
            }
            else {
                $scope.alert('Alert !!', "Update not Success" + response.data.Message, 'btn-danger', 'red');
            }
        });
    }

    $scope.DsgnCheck = function () {
        if ($scope.Edtbydesign == true) {
            $scope.alert('Alert !!', "After Updation of costing you can make Changes", 'btn-danger', 'red');
        }
    }
    $scope.alert2 = function (title, msg, btnclss, typeclr, confirmCallback) {
        $ngConfirm({
            title: title,
            content: msg,
            type: typeclr,
            typeAnimated: true,
            buttons: {
                confirm: {
                    text: 'Any way',
                    btnClass: 'btn-primary', // You can change this to the desired button class
                    action: function () {
                        if (confirmCallback && typeof confirmCallback === 'function') {
                            confirmCallback(); // Call the callback function if provided
                        }

                    }
                },
                close: {
                    text: 'Cancel',
                    btnClass: btnclss,
                    action: function () {
                        // Optionally handle cancel action here
                    }
                }
            }
        });
    };

    $scope.containsUserIdCRR2 = function (id) {
        return $scope.Filearr.some(function (item) {
            return item.UserId === id;
        });
    };
    $scope.SFG_Item_creare_function = function (Doc) {
        $scope.SFG_Item = {};
        $scope.SFG_Item.last_DocEntry = Doc;
        $scope.SFG_Item.Production_Process = $scope.Production_Process_Detail;
        $http.post('/Home/SFG_Item_Create', $scope.SFG_Item).then(function (response) {
            if (response.data != "0") {

                $scope.alert('Alert !!', "SFG Item Created for  '" + Doc + "' DocEntry ", 'btn-success', 'green');
                window.addEventListener('click', function () {
                    location.reload();
                });

            }
            else {
                $scope.alert('Alert !!', "Submition Failed", 'btn-danger', 'red');

            }

        })
    }
    //All Material Detail
    $scope.showupdbtn = function () {
        $scope.updbtn = true;
    }
    // Seacrch Customer Detail
    $scope.Search_Data = function () {
        try {
            $http.get("/Home/FindCustomer?SessiouserDep=" + SessiouserDep).then(function (res) {

                if (res != null) {
                    $scope.Alltowelinfo1 = res.data.abc;
                    $scope.usersTable = new NgTableParams({}, { dataset: res.data.abc });

                    //$scope.alert('', "Find Success", 'btn-success', 'green', 'close');
                }
                if (res == null) {
                    $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
                }
            });

        }
        catch (ex) {

        }

    }
    $scope.Approved_Chng = (Revise_val) => {
        if (Revise_val == true)
            $scope.BtnName = "Submit";
        else
            $scope.BtnName = "Update";
    }
    $scope.convertDateFormat = function (dateStr) {
        const [day, month, year] = dateStr.split('-');
        return `${year}-${month}-${day}`;
    };
    //Edit Function for all

    $scope.EditAll = function (docentry, status) {

        try {
            if (docentry != null) {
                var docentryint = parseInt(docentry);
                $scope.materiallist = false;
                $scope.Afterclickcosttype = true;
                $scope.angodiv = false;
                $scope.namediv = true;
                $scope.BtnName = "Update";
                $scope.DocEntry_val = docentry;
                $scope.Costing_num_at_Edit = false;
                $scope.Costing_num_at_Insert = true;
                $scope.date_on_edit = false;
                $scope.date_on_Insert = true;
                $scope.RenewvalBtn = false;
                $scope.Mat = [];
                $scope.Costing_status = status;
                $http.get("/Home/EditAll?docentry=" + docentryint).then(function (res) {
                    //$scope.UserDisblAth = res.data.User_Oth[0].U_Dpt;
                    $scope.CustomerName = res.data.abc3[0].Customername;
                    $scope.CustomerCode = res.data.abc3[0].CustomerCode;
                    $scope.Alltowelinfo1.cdate = res.data.abc3[0].CostingDate;
                    $scope.Alltowelinfo1.Dyed_Type = res.data.abc3[0].DYED_VAL;
                    $scope.Alltowelinfo1.Weaving_Type = res.data.abc3[0].Weaving_VAL;
                    $scope.Alltowelinfo1.Costing_Num = res.data.abc3[0].CostingNum;
                    $scope.Alltowelinfo1.SaleNum = parseInt(res.data.abc3[0].SaleNum);
                    $scope.Alltowelinfo1.Merch_Status = res.data.abc3[0].Merch_Status;
                    $scope.Alltowelinfo1.HRem = res.data.abc3[0].HRem;
                    if (res.data.abc3[0].SaleNum != '') {
                        $scope.Alltowelinfo1.SalSrType = "Manual";
                    }
                    $scope.merchandiser = res.data.abc3[0].merchandiser;
                    $scope.merchandiserCode = res.data.abc3[0].merchandiserCode;
                    $scope.CRefNo = res.data.abc3[0].CRefNo;
                    $scope.General_Inputs = res.data.GENRAL_INPUTS[0];
                    for (var i = 0; i < res.data.BATHSHEETARRY.length; i++) {
                        var item = res.data.BATHSHEETARRY[i];
                        item.Length = parseFloat(item.Length) || 0;
                        item.Width = parseFloat(item.Width) || 0;
                        item.GSMlbsdoz = parseFloat(item.GSMlbsdoz) || 0;
                        item.PcWeight = parseFloat(item.PcWeight) || 0;
                        item.Qty = parseFloat(item.Qty) || 0;
                        item.TotalKg = parseFloat(item.TotalKg) || 0;
                        item.Pricepc = parseFloat(item.Pricepc) || 0;
                        item.DyWgt = parseFloat(item.DyWgt) || 0;
                        item.Shearingloss = parseFloat(item.Shearingloss) || 0;
                        item.GryWgt = parseFloat(item.GryWgt) || 0;
                        item.WaveLossper = parseFloat(item.WaveLossper) || 0;
                        item.planQty = parseFloat(item.planQty) || 0;
                        item.planTotalKg = parseFloat(item.planTotalKg) || 0;
                        item.OrderplanTotalper = parseFloat(item.OrderplanTotalper) || 0;
                        item.PriceKgs = parseFloat(item.PriceKgs) || 0;
                        item.TotalWaveLoss = parseFloat(item.TotalWaveLoss) || 0;
                    }
                    $scope.Alltowelinfo1.BathSheetArry = res.data.BATHSHEETARRY;
                    if (res.data.BATHSHEETARRY.length != 0) {
                        $scope.AddRow_Totals();
                    }
                    if (res.data.DesignYarnInformation.length != 0) {

                        //$scope.DesignYarnInformation = res.data.DesignYarnInformation;
                        $scope.DesignYarnInformation = res.data.DesignYarnInformation.map(function (item) {
                            // Convert the fields to float, handle invalid or empty values by using NaN or a fallback value (e.g., 0)
                            item.PerPc_Yarn_Consumed_in_Mtr = parseFloat(item.PerPc_Yarn_Consumed_in_Mtr) || 0;  // Default to 0 if conversion fails
                            item.PerPc_Yarn_Consumed_in_Kg = parseFloat(item.PerPc_Yarn_Consumed_in_Kg) || 0;
                            item.Tot_Yarn_Consumed_in_Mtr = parseFloat(item.Tot_Yarn_Consumed_in_Mtr) || 0;
                            item.Tot_Yarn_Consumed_in_Kg = parseFloat(item.Tot_Yarn_Consumed_in_Kg) || 0;
                            return item;
                        });
                    }
                    $scope.Production_Process_Detail = res.data.Production_Process;
                    if ($scope.Production_Process_Detail.length != 0) {
                        for (var i = 0; i < $scope.Production_Process_Detail.length; i++) {
                            for (var j = 0; j < $scope.Production_Process.length; j++) {
                                if ($scope.Production_Process_Detail[i].U_Process_Code == $scope.Production_Process[j].U_Process_Code) {
                                    $scope.Production_Process[j].selected = true;
                                    $scope.Production_Process[j].U_Process_Status = "Yes";

                                }
                            }

                        }
                    }
                    $scope.Mat = res.data.ASSUMPTION_DETAILS;
                    for (var i = 0; i < $scope.Mat.length; i++) {
                        $scope.Mat[i].Unit_Price = parseFloat($scope.Mat[i].Unit_Price);
                        $scope.Mat[i].Quantity = parseFloat($scope.Mat[i].Quantity);
                        $scope.Mat[i].Material_Value = parseFloat($scope.Mat[i].Material_Value);
                        if ($scope.Mat[i].Material_Code == '28') {
                            $scope.SAL_Total_Price = $scope.Mat[i].Material_Value
                        }
                    }
                    if ($scope.Allvalues.length != 0) {
                        $scope.Allvalues.splice(0, $scope.Allvalues.length);
                        Percentage_ = parseFloat(res.data.abc2[i].Percentage)
                        Percentage_val_ = parseFloat(res.data.abc2[i].Percentage_val)
                        for (var i = 0; i < res.data.abc2.length; i++) {
                            costdata_ = parseFloat(res.data.abc2[i].costdata)
                            $scope.Allvalues.push({
                                Type: res.data.abc2[i].Type,
                                SD_VAL: res.data.abc2[i].SD_VAL,
                                countdata: res.data.abc2[i].countdata,
                                card_cmbd_OE: res.data.abc2[i].card_cmbd_OE,
                                costdata: costdata_,
                                Percentage: Percentage_,
                                Percentage_val: Percentage_val_,
                                YarnRem: res.data.abc2[i].YarnRem
                            });
                        }
                        $scope.Allinfo_total();
                    }
                    else {
                        for (var i = 0; i < res.data.abc2.length; i++) {
                            costdata_ = parseFloat(res.data.abc2[i].costdata)
                            Percentage_ = parseFloat(res.data.abc2[i].Percentage)
                            Percentage_val_ = parseFloat(res.data.abc2[i].Percentage_val)
                            $scope.Allvalues.push({
                                Type: res.data.abc2[i].Type,
                                SD_VAL: res.data.abc2[i].SD_VAL,
                                countdata: res.data.abc2[i].countdata,
                                card_cmbd_OE: res.data.abc2[i].card_cmbd_OE,
                                costdata: costdata_,
                                Percentage: Percentage_,
                                Percentage_val: Percentage_val_,
                                YarnRem: res.data.abc2[i].YarnRem
                            });
                        }
                        $scope.Allinfo_total();
                    }
                    $scope.AuditDep = res.data.AuditStatus;
                    $scope.Filearr = res.data.FileDAta;
                    $scope.Commission_per = parseFloat(res.data.abc3[0].Commition);
                    $scope.Ocean_freight_val = parseFloat(res.data.abc3[0].Oceanfreightinclddc);
                    $scope.Price = parseFloat(res.data.abc3[0].Contractprice);
                    $scope.electricityval = parseFloat(res.data.abc3[0].Electricity);
                    $scope.Streamval = parseFloat(res.data.abc3[0].Stream);
                    $scope.Mktgval = parseFloat(res.data.abc3[0].MktgExpenses);
                    $scope.valuelossval = parseFloat(res.data.abc3[0].ValueLoss);
                    $scope.commitionval = parseFloat(res.data.abc3[0].Commition);
                })
            }
        }
        catch (ex) {

        }

    }

    //Add input data kartik

    $scope.Allvalues = [];
    $scope.ADD = function () {
        $scope.T_info_headings = false;
        $scope.Hide_btn_Dlt_tinfo = false;
        if ($scope.Type !== undefined && $scope.countdata !== "") {
            // Check if the combination already exists in the array
            var combinationExists = $scope.Allvalues.some(function (item) {
                return item.Type === $scope.Type && item.countdata === $scope.countdata && item.card_cmbd_OE === $scope.card_cmbd_OE;
            });

            if (!combinationExists) {
                $scope.Allvalues.push({
                    Type: $scope.Type,
                    Percentage: $scope.Percentage,
                    SD_VAL: $scope.SD_VAL,
                    countdata: $scope.countdata,
                    card_cmbd_OE: $scope.card_cmbd_OE,
                    costdata: $scope.costdata,
                    Percentage_val: $scope.costdata_percentage,
                    YarnRem: $scope.YarnRem
                });
                //$scope.S_D_Sizing();
            } else {
                // Display an alert message for the duplicate combination
                alert("This combination already exists in the Towel Information.");
            }

            $scope.Allinfo_total();

        }
        else {
            $scope.alert('Alert !!', "Plsease Fill all Fields", 'btn-danger', 'red');
        }
        $scope.Edtbydesign = true;
        $scope.Type = "";
        $scope.Percentage = "";
        $scope.countdata = "";
        $scope.costdata_percentage = "";
        $scope.costdata = "";
        $scope.YarnRem = "";
        //angular.element('#Items').scope().selectedItem = null;
        document.getElementById("txttinfo").focus();
    };

    ////////////////////////////////////////////// Assumption Price multiply Quantity Function //////////////////////////////////////////////////////////

    $scope.Assumption_UP_QTY = function (i) {
        for (var a = 0; a < $scope.Mat.length; a++) {
            if (i == $scope.Mat[a].Material_Code) {
                $scope.Mat[a].Material_Value = parseFloat(($scope.Mat[a].Unit_Price * $scope.Mat[a].Quantity).toFixed(2));
            }
        }
    }

    $scope.percentage = function () {
        if ($scope.Percentage_total_Tinfo != undefined) {
            $scope.Percentage_total_Tinfo_check = $scope.Percentage + $scope.Percentage_total_Tinfo;
        }
        if ($scope.Percentage_total_Tinfo_check <= 100 || $scope.Percentage_total_Tinfo_check == undefined) {
            if ($scope.Percentage <= 100) {
                $scope.costdata_percentage = parseFloat((($scope.costdata * $scope.Percentage) / 100).toFixed(2));
            }
            else {
                $scope.alert('Alert !!', "Percentage Value Should be less than 100 !!", 'btn-danger', 'red');
                $scope.Percentage = '';
            }
        } else {
            $scope.alert('Alert !!', "Total Percentage Should be less than 100 !!", 'btn-danger', 'red');
            $scope.Percentage = '';
        }

    }
    $scope.Allinfo_total = function () {
        $scope.Percentage_total_Tinfo = 0.0;
        $scope.costdata_Percentage_total_Tinfo = 0.0;
        for (var i = 0; i < $scope.Allvalues.length; i++) {
            $scope.Percentage_total_Tinfo = parseFloat(($scope.Percentage_total_Tinfo + $scope.Allvalues[i].Percentage).toFixed(2));
            $scope.costdata_Percentage_total_Tinfo = parseFloat(($scope.costdata_Percentage_total_Tinfo + $scope.Allvalues[i].Percentage_val).toFixed(2));
        }
        if ($scope.Alltowelinfo1.BathSheetArry.length != 0) {
            $scope.AddRow_Totals();
        }
    }
    $scope.poly_Autofocus = function () {
        $scope.General_Inputs.POLYESTER_CODE = "";
        var textBox = document.getElementById("txtpoly");
        textBox.style.border = "2px solid red";
        textBox.focus();
    }
    $scope.delete = function (res) {
        //$scope.unselectDD_polyester();
        //$scope.Row_yarn = parseFloat($scope.Row_yarn - ($scope.Allvalues[res].costdata * $scope.Allvalues[res].Percentage)).toFixed(2);

        $scope.Percentage_total_Tinfo = parseFloat(($scope.Percentage_total_Tinfo - $scope.Allvalues[res].Percentage).toFixed(2));
        $scope.costdata_Percentage_total_Tinfo = parseFloat(($scope.costdata_Percentage_total_Tinfo - $scope.Allvalues[res].Percentage_val).toFixed(2));
        $scope.Allvalues.splice(res, 1);
        $scope.Edtbydesign = true;
        //$scope.S_D_Sizing();
        //$scope.Costing_Calculation();

    };

    //////////////////// Current_Exchange_Rate 
    $scope.Current_Exchange_Rate = function () {

        $http.get("/Home/Current_Exchange_Rate?a=" + $scope.General_Inputs.Currency_Code).then(function (res) {
            if (res.data.length != 0) {
                $scope.General_Inputs.Current_Exchange_Rate_val = parseFloat(res.data[0].Rate);
                // $scope.comm_cal();
                for (var i = 0; i < $scope.Mat.length; i++) {
                    if ($scope.Mat[i].Material_name == "Exchange Rate") {
                        $scope.Mat[i].Material_Value = $scope.General_Inputs.Current_Exchange_Rate_val;
                    }
                }

            }
            else {
                $scope.alert('Alert !!', "Please Update Your Exchange Rate Table", 'btn-danger', 'red');
            }
        });
    }

    $scope.focusOnTextbox = function (index) {
        // Use the index to identify the specific text box to focus on
        var textboxId = 'txtmat' + index;
        var element = document.getElementById(textboxId);
        if (element) {
            element.focus();
        }
    };

    $scope.PrintRepaort = true;
    //Repot Print
    $scope.PrintRep = (elem) => {
        $scope.PrintRepaort = false;

        var printElement = document.getElementById(elem);

        if (printElement) {
            var printContents = printElement.innerHTML;
            var originalContents = document.body.innerHTML;

            // Open a new window for printing
            var printWindow = window.open('', '_blank');

            // Write the HTML content to the new window
            printWindow.document.write('<html><head><title>' + document.title + '</title></head><body>');
            printWindow.document.write(printContents);
            printWindow.document.write('</body></html>');

            // Print and close the new window
            printWindow.document.close();
            printWindow.print();
            printWindow.onafterprint = function () {
                printWindow.close();
            };

            // Restore the original content to the main window
            document.body.innerHTML = originalContents;
            location.reload();
        } else {
            console.error("Element with ID '" + elem + "' not found.");
        }
    };
    //dlt input data 

    $scope.Edit_Towel_info = function (i) {
        //$scope.unselectDD_polyester();
        $scope.Hide_btn_Dlt_tinfo = true;
        $scope.Hide_btn_ud_tinfo = false;
        $scope.Type = $scope.Allvalues[i].Type;
        $scope.SD_VAL = $scope.Allvalues[i].SD_VAL;
        $scope.countdata = $scope.Allvalues[i].countdata;
        $scope.card_cmbd_OE = $scope.Allvalues[i].card_cmbd_OE;
        $scope.costdata = $scope.Allvalues[i].costdata;
        $scope.costdata_percentage = $scope.Allvalues[i].Percentage_val;
        $scope.YarnRem = $scope.Allvalues[i].YarnRem;
        $scope.Percentage_total_Tinfo = parseFloat(($scope.Percentage_total_Tinfo - $scope.Allvalues[i].Percentage).toFixed(2));
        $scope.costdata_Percentage_total_Tinfo = parseFloat(($scope.costdata_Percentage_total_Tinfo - $scope.Allvalues[i].Percentage_val).toFixed(2));
        $scope.Edittowel_index = i;
    }

    $scope.Update_towel_info = function () {
        $scope.Hide_btn_Dlt_tinfo = false;
        $scope.Allvalues[$scope.Edittowel_index].Type = $scope.Type;
        $scope.Allvalues[$scope.Edittowel_index].Percentage = $scope.Percentage;
        $scope.Allvalues[$scope.Edittowel_index].SD_VAL = $scope.SD_VAL;
        $scope.Allvalues[$scope.Edittowel_index].countdata = $scope.countdata;
        $scope.Allvalues[$scope.Edittowel_index].card_cmbd_OE = $scope.card_cmbd_OE;
        $scope.Allvalues[$scope.Edittowel_index].costdata = $scope.costdata;
        $scope.Allvalues[$scope.Edittowel_index].Percentage_val = $scope.costdata_percentage;
        $scope.Allvalues[$scope.Edittowel_index].YarnRem = $scope.YarnRem;
        $scope.Allinfo_total();
        $scope.Edtbydesign = true;
        //$scope.S_D_Sizing();
        $scope.Type = "";
        $scope.Percentage = "";
        $scope.SD_VAL = "";
        $scope.countdata = "";
        $scope.card_cmbd_OE = "";
        $scope.costdata = "";
        $scope.costdata_percentage = "";
        $scope.YarnRem = "";
        document.getElementById("txttinfo").focus();
    }

    $scope.SalectTypeCode = function (Tname) {
        try {

            angular.element('#loading').show();
            $http.get('/api/WebAPI/GetCodeValue?Type=' + Tname).then(function (response) {
                $scope.Alltowelinfo1.Code = response.data[0].Code;
                $scope.Tname = $scope.Alltowelinfo1.Tname;
                $scope.Tcode = $scope.Alltowelinfo1.Code
                angular.element('#loading').hide();

            }).catch(function (response) {
                $scope.alert('Alert !!', response.data, 'btn-danger', 'red');
                console.log(response.data);
            })
        }
        catch (ex) {
            $scope.HideLoader();
            $scope.alert('Alert !!', ex, 'btn-danger', 'red');
            console.log(ex);
        }
    };

    $scope.AddRow = function () {


        if ($scope.AddValidate()) {
            $scope.Terry_Towels.Length = $scope.Terry_Towels.Length;
            $scope.Terry_Towels.Width = $scope.Terry_Towels.Width;
            if ($scope.Alltowelinfo1.BathSheetArry == undefined) {
                $scope.Alltowelinfo1.BathSheetArry = [];
            }

            $scope.Alltowelinfo1.BathSheetArry.push($scope.Terry_Towels);
            $scope.Alltowelinfo1.BathSheetArry = $scope.Alltowelinfo1.BathSheetArry.slice().reverse();

            $scope.Terry_Towels = {};
            document.getElementById("txtbathsheet").focus();
        }
        else {
            $scope.alert('Alert !!', 'Please Fill Required Fields', 'btn-danger', 'red');
        }
        $scope.AddRow_Totals();

    };
    $scope.AddRow_Totals = function () {
        // Initialize totals
        $scope.Grand_Total_kg = 0;
        $scope.Grand_Total_pc = 0;
        $scope.Grand_Total_Qty = 0;
        $scope.Grand_Total_GSM_Qty = 0;
        $scope.Grand_Total_GSM_Qty_AVG = 0;
        $scope.Wavesloss_Total = 0;
        $scope.Graywt_Total = 0;
        $scope.wtloss_Total_per = 0;
        $scope.T.Yarn_Total_Val = 0; // Adjusted from 1 to 0
        $scope.planQty_Total = 0;
        $scope.planQty_Total_Kg = 0;

        if (!$scope.Mat) {
            $scope.Mat = [];
        }

        for (var i = 0; i < $scope.Alltowelinfo1.BathSheetArry.length; i++) {
            var item = $scope.Alltowelinfo1.BathSheetArry[i];

            // Safely parse values
            var totalKg = parseFloat(item.TotalKg) || 0;
            var pricePc = parseFloat(item.Pricepc) || 0;
            var qty = parseFloat(item.Qty) || 0;
            var gsmLbsdoz = parseFloat(item.GSMlbsdoz) || 0;
            var totalWaveLoss = parseFloat(item.TotalWaveLoss) || 0;
            var grayWgt = parseFloat(item.GryWgt) || 0;
            var waveLossper = parseFloat(item.WaveLossper) || 0;
            var planQty = parseFloat(item.planQty) || 0;
            var planTotalKg = parseFloat(item.planTotalKg) || 0;

            // Calculate totals
            $scope.Grand_Total_kg += totalKg;
            $scope.Grand_Total_pc += pricePc;
            $scope.Grand_Total_Qty += qty;
            $scope.Grand_Total_GSM_Qty += (gsmLbsdoz * qty);
            $scope.Wavesloss_Total += parseFloat((totalWaveLoss).toFixed(2));
            $scope.Graywt_Total += grayWgt;
            $scope.wtloss_Total_per += waveLossper;
            $scope.T.Yarn_Total_Val = parseFloat(($scope.Wavesloss_Total * ($scope.costdata_Percentage_total_Tinfo || 0)).toFixed(2));

            // Initialize the Mat array if not already done
            if (!$scope.Mat[0]) {
                $scope.Mat[0] = {};
            }
            $scope.Mat[0].Material_Value = $scope.T.Yarn_Total_Val;
            $scope.Mat[0].Quantity = parseFloat(($scope.Wavesloss_Total).toFixed(2));
            $scope.Mat[0].Unit_Price = $scope.costdata_Percentage_total_Tinfo;

            // Update plan totals
            $scope.planQty_Total += planQty;
            $scope.planQty_Total_Kg += planTotalKg;
        }
        if ($scope.Alltowelinfo1.Dyed_Type == 'PIECE DYED') {
            var totalWaveLoss = parseFloat($scope.Alltowelinfo1.BathSheetArry[0].TotalWaveLoss) || 0;
            if (!$scope.Mat[1]) {
                $scope.Mat[1] = {};
            }
            if (!$scope.Mat[2]) {
                $scope.Mat[2] = {};
            }
            // Swapping the assignments
            $scope.Mat[1].Quantity = parseFloat(($scope.Mat[0].Quantity).toFixed(2));
            //$scope.Mat[1].Quantity = parseFloat((totalWaveLoss).toFixed(2));
            /*$scope.Mat[2].Quantity = parseFloat(($scope.Wavesloss_Total * ((100 - $scope.Alltowelinfo1.BathSheetArry[0].DyWgt) / 100)).toFixed(2));*/
            //$scope.Mat[2].Quantity = parseFloat(($scope.Alltowelinfo1.BathSheetArry[0].TotalWaveLoss * ((100 - $scope.Alltowelinfo1.BathSheetArry[0].DyWgt) / 100)).toFixed(2));
            $scope.Mat[2].Quantity = parseFloat(($scope.Mat[0].Quantity * ((100 - $scope.Alltowelinfo1.BathSheetArry[0].WaveLossper) / 100)).toFixed(2));
        }
        // Format the final results with toFixed
        $scope.Grand_Total_kg = parseFloat($scope.Grand_Total_kg.toFixed(2));
        $scope.Grand_Total_pc = parseFloat($scope.Grand_Total_pc.toFixed(2));
        $scope.Grand_Total_GSM_Qty = parseFloat($scope.Grand_Total_GSM_Qty.toFixed(2));
        $scope.Grand_Total_GSM_Qty_AVG = $scope.Grand_Total_Qty > 0 ? parseFloat(($scope.Grand_Total_GSM_Qty / $scope.Grand_Total_Qty).toFixed(2)) : 0;
        $scope.Wavesloss_Total = parseFloat($scope.Wavesloss_Total.toFixed(2));
        $scope.Graywt_Total = parseFloat($scope.Graywt_Total.toFixed(2));
        $scope.wtloss_Total_per = parseFloat($scope.wtloss_Total_per.toFixed(2));
        $scope.planQty_Total = parseFloat($scope.planQty_Total.toFixed(2));
        $scope.planQty_Total_Kg = parseFloat($scope.planQty_Total_Kg.toFixed(2));

        if ($scope.Alltowelinfo1.Dyed_Type == 'YARN DYED') {
            if (!$scope.Mat[1]) {
                $scope.Mat[1] = {};
            }
            if (!$scope.Mat[2]) {
                $scope.Mat[2] = {};
            }
            // Swapping the assignments
            /*$scope.Mat[1].Quantity = $scope.planQty_Total;*/
            $scope.Mat[2].Quantity = parseFloat(($scope.Mat[0].Quantity).toFixed(2));
            /*$scope.Mat[1].Quantity = parseFloat(($scope.Wavesloss_Total * ((100 - $scope.Alltowelinfo1.BathSheetArry[0].DyWgt) / 100)).toFixed(2));*/
            $scope.Mat[1].Quantity = parseFloat(($scope.Mat[0].Quantity * ((100 - $scope.Alltowelinfo1.BathSheetArry[0].DyWgt) / 100)).toFixed(2));
        }
    }

    $scope.Editdata = function (index) {
        $scope.Alltowelinfo1.BathSheetArry[index].color = "edit";
        $scope.Updatebtn = true;
        try {
            if ($scope.Alltowelinfo1.BathSheetArry.length > 0) {
                $scope.Terry_Towels = $scope.Alltowelinfo1.BathSheetArry[index]
                $scope.showSavebutton = true;
                for (var i = 0; i < $scope.Alltowelinfo1.BathSheetArry.length; i++) {
                    if (i == index)
                        $scope.Alltowelinfo1.BathSheetArry[i].HideButton = true;
                    else
                        $scope.Alltowelinfo1.BathSheetArry[i].HideButton = false;
                }
            }
            $scope.showSavebutton = true;
            $scope.index1 = index;
            $scope.Grand_Total_kg = $scope.Grand_Total_kg - $scope.Alltowelinfo1.BathSheetArry[$scope.index1].TotalKg;
            $scope.Grand_Total_pc = $scope.Grand_Total_pc - $scope.Alltowelinfo1.BathSheetArry[$scope.index1].Pricepc;
            $scope.Grand_Total_Qty = parseFloat(($scope.Grand_Total_Qty - $scope.Alltowelinfo1.BathSheetArry[$scope.index1].Qty).toFixed(2));
        }
        catch (ex) {
            $scope.alert('Alert !!', ex, 'btn-danger', 'red');
            console.log(ex);
        }

    };

    $scope.Update2 = function () {

        $scope.Alltowelinfo1.BathSheetArry[$scope.index1] = $scope.Terry_Towels;
        $scope.Alltowelinfo1.BathSheetArry[$scope.index1].color = "";
        $scope.Grand_Total_kg = $scope.Grand_Total_kg + $scope.Terry_Towels.TotalKg;
        $scope.Grand_Total_pc = $scope.Grand_Total_pc + $scope.Terry_Towels.Pricepc;
        $scope.Grand_Total_Qty = $scope.Grand_Total_Qty + $scope.Terry_Towels.Qty;
        $scope.AddRow_Totals();
        $scope.Terry_Towels = {};
    }

    $scope.Deletemain = function (res, Tn) {
        $scope.Grand_Total_kg = parseFloat(($scope.Grand_Total_kg - ($scope.Alltowelinfo1.BathSheetArry[res].TotalKg)).toFixed(2));
        $scope.Grand_Total_pc = parseFloat(($scope.Grand_Total_pc - $scope.Alltowelinfo1.BathSheetArry[res].Pricepc).toFixed(2));
        $scope.Grand_Total_Qty = parseFloat(($scope.Grand_Total_Qty - $scope.Alltowelinfo1.BathSheetArry[res].Qty).toFixed(2));
        $scope.Alltowelinfo1.BathSheetArry.splice(res, 1);
        $scope.AddRow_Totals();
    }



    $scope.Electicity_cal = function () {
        //Total Units According to per KG
        $scope.Grand_Total_Unit = $scope.Grand_Total_kg * 4;
        //Total Units Rate According to per KG  

        for (var i = 0; i < $scope.Mat.length; i++) {

            if ($scope.Mat[i].Material_name == "Electricity  Rs. 5.12/ unit") {
                if ($scope.Count_var == undefined) {
                    $scope.Count_var_val = $scope.Mat[i].Material_Value;
                }
                $scope.Mat[i].Material_Value = parseFloat(parseFloat($scope.Electicity_Int_val) * $scope.Grand_Total_Unit).toFixed(3);
            }
            if ($scope.Mat[i].Material_name == "Steam  Rs. .5 per kg") {
                if ($scope.Count_var == undefined) {
                    $scope.Count_var_val2 = $scope.Mat[i].Material_Value;
                }
                $scope.Mat[i].Material_Value = parseFloat(parseFloat($scope.steam_Int_val) * $scope.Grand_Total_kg).toFixed(3);
            }


        }
        $scope.Count_var = $scope.Count_var + 1;
    }


    $scope.AddRow1 = function () {
        if ($scope.AddValidate1()) {
            $scope.Terry_Towels.Length1 = $scope.Terry_Towels.Length1;
            $scope.Terry_Towels.Width1 = $scope.Terry_Towels.Width1;
            if ($scope.Alltowelinfo1.BathTowelArry == undefined) {
                $scope.Alltowelinfo1.BathTowelArry = [];
            }
            $scope.Alltowelinfo1.BathTowelArry.push($scope.Terry_Towels);
            $scope.Terry_Towels = {};
            document.getElementById("txtbathtowel").focus();
            if ($scope.BathTowelArry.length > 0) {
                $scope.BathTowelArry.HideButton = false;
                $scope.$broadcast('angucomplete-alt:clearInput', 'Length');
                $scope.$broadcast('angucomplete-alt:clearInput', 'Width');
            }
        }
        else {
            $scope.alert('Alert !!', 'Please Fill Required Fields', 'btn-danger', 'red');
        }

        $scope.Grand_Total_kg1 = 0;
        $scope.Grand_Total_pc1 = 0;
        for (var i = 0; i < $scope.Alltowelinfo1.BathTowelArry.length; i++) {
            $scope.Grand_Total_kg1 = $scope.Grand_Total_kg1 + $scope.Alltowelinfo1.BathTowelArry[i].TotalKg1;
            $scope.Grand_Total_pc1 = $scope.Grand_Total_pc1 + $scope.Alltowelinfo1.BathTowelArry[i].Pricepc1;
        }

    };

    $scope.AddRow2 = function () {
        if ($scope.AddValidate2()) {
            $scope.Terry_Towels.Length2 = $scope.Terry_Towels.Length2;
            $scope.Terry_Towels.Width2 = $scope.Terry_Towels.Width2;
            if ($scope.Alltowelinfo1.HandTowelArry == undefined) {
                $scope.Alltowelinfo1.HandTowelArry = [];
            }
            $scope.Alltowelinfo1.HandTowelArry.push($scope.Terry_Towels);
            if ($scope.Alltowelinfo1.HandTowelArry.length > 0) {
                $scope.Terry_Towels = {};
                document.getElementById("txtbhandtowel").focus();
                $scope.HandTowelArry.HideButton = false;
                $scope.$broadcast('angucomplete-alt:clearInput', 'Length');
                $scope.$broadcast('angucomplete-alt:clearInput', 'Width');
            }
        }
        else {
            $scope.alert('Alert !!', 'Please Fill Required Fields', 'btn-danger', 'red');
        }

        $scope.Grand_Total_kg2 = 0;
        $scope.Grand_Total_pc2 = 0;
        for (var i = 0; i < $scope.Alltowelinfo1.HandTowelArry.length; i++) {
            $scope.Grand_Total_kg2 = $scope.Grand_Total_kg2 + $scope.Alltowelinfo1.HandTowelArry[i].TotalKg2;
            $scope.Grand_Total_pc2 = $scope.Grand_Total_pc2 + $scope.Alltowelinfo1.HandTowelArry[i].Pricepc2;
        }
    };

    $scope.AddRow3 = function () {
        if ($scope.AddValidate3()) {
            $scope.Terry_Towels.Length3 = $scope.Terry_Towels.Length3;
            $scope.Terry_Towels.Width3 = $scope.Terry_Towels.Width3;

            if ($scope.Alltowelinfo1.WashClotharry == undefined) {
                $scope.Alltowelinfo1.WashClotharry = [];
            }
            $scope.Alltowelinfo1.WashClotharry.push($scope.Terry_Towels);
            if ($scope.Alltowelinfo1.WashClotharry.length > 0) {
                $scope.Terry_Towels = {};
                document.getElementById("txtWashcloth").focus();
                $scope.WashClotharry.HideButton = false;
                $scope.$broadcast('angucomplete-alt:clearInput', 'Length');
                $scope.$broadcast('angucomplete-alt:clearInput', 'Width');
            }
        }
        else {
            $scope.alert('Alert !!', 'Please Fill Required Fields', 'btn-danger', 'red');
        }
        $scope.Grand_Total_kg3 = 0;
        $scope.Grand_Total_pc3 = 0;
        for (var i = 0; i < $scope.Alltowelinfo1.WashClotharry.length; i++) {
            $scope.Grand_Total_kg3 = $scope.Grand_Total_kg3 + $scope.Alltowelinfo1.WashClotharry[i].TotalKg3;
            $scope.Grand_Total_pc3 = $scope.Grand_Total_pc3 + $scope.Alltowelinfo1.WashClotharry[i].Pricepc3;
        }
    };

    $scope.AddRow4 = function () {
        if ($scope.AddValidate4()) {
            $scope.Terry_Towels.Length4 = $scope.Terry_Towels.Length4;
            $scope.Terry_Towels.Width4 = $scope.Terry_Towels.Width4;
            if ($scope.Alltowelinfo1.WashGlovearry == undefined) {
                $scope.Alltowelinfo1.WashGlovearry = [];
            }
            $scope.Alltowelinfo1.WashGlovearry.push($scope.Terry_Towels);
            if ($scope.Alltowelinfo1.WashGlovearry.length > 0) {
                $scope.Terry_Towels = {};
                document.getElementById("txtWashglove").focus();
                $scope.WashGlovearry.HideButton = false;
                $scope.$broadcast('angucomplete-alt:clearInput', 'Length');
                $scope.$broadcast('angucomplete-alt:clearInput', 'Width');
            }
        }
        else {
            $scope.alert('Alert !!', 'Please Fill Required Fields', 'btn-danger', 'red');
        }

        $scope.Grand_Total_kg4 = 0;
        $scope.Grand_Total_pc4 = 0;
        for (var i = 0; i < $scope.Alltowelinfo1.WashGlovearry.length; i++) {
            $scope.Grand_Total_kg4 = $scope.Grand_Total_kg4 + $scope.Alltowelinfo1.WashGlovearry[i].TotalKg4;
            $scope.Grand_Total_pc4 = $scope.Grand_Total_pc4 + $scope.Alltowelinfo1.WashGlovearry[i].Pricepc4;
        }

    };



    $scope.Editdata1 = function (index) {
        try {
            if ($scope.Alltowelinfo1.BathTowelArry.length > 0) {
                $scope.Terry_Towels = $scope.Alltowelinfo1.BathTowelArry[index]
                $scope.showSavebutton = true;
                for (var i = 0; i < $scope.Alltowelinfo1.BathTowelArry.length; i++) {
                    if (i == index)
                        $scope.Alltowelinfo1.BathTowelArry[i].HideButton = true;
                    else
                        $scope.Alltowelinfo1.BathTowelArry[i].HideButton = false;
                }
            }
            $scope.showSavebutton = true;
            $scope.index2 = index;
            $scope.Grand_Total_kg1 = $scope.Grand_Total_kg1 - $scope.Alltowelinfo1.BathTowelArry[$scope.index2].TotalKg1;
            $scope.Grand_Total_pc1 = $scope.Grand_Total_pc1 - $scope.Alltowelinfo1.BathTowelArry[$scope.index2].Pricepc1;

        }
        catch (ex) {
            $scope.alert('Alert !!', ex, 'btn-danger', 'red');
            console.log(ex);
        }

    };

    $scope.Editdata2 = function (index) {
        try {
            if ($scope.Alltowelinfo1.HandTowelArry.length > 0) {
                $scope.Terry_Towels = $scope.Alltowelinfo1.HandTowelArry[index]
                $scope.showSavebutton = true;
                for (var i = 0; i < $scope.Alltowelinfo1.HandTowelArry.length; i++) {
                    if (i == index)
                        $scope.Alltowelinfo1.HandTowelArry[i].HideButton = true;
                    else
                        $scope.Alltowelinfo1.HandTowelArry[i].HideButton = false;
                }
            }
            $scope.showSavebutton = true;
            $scope.index3 = index;
            $scope.Grand_Total_kg2 = $scope.Grand_Total_kg2 - $scope.Alltowelinfo1.HandTowelArry[$scope.index3].TotalKg2;
            $scope.Grand_Total_pc2 = $scope.Grand_Total_pc2 - $scope.Alltowelinfo1.HandTowelArry[$scope.index3].Pricepc2;
        }
        catch (ex) {
            $scope.alert('Alert !!', ex, 'btn-danger', 'red');
            console.log(ex);
        }

    };

    $scope.Editdata3 = function (index) {
        try {
            if ($scope.Alltowelinfo1.WashClotharry.length > 0) {
                $scope.Terry_Towels = [];
                $scope.Terry_Towels = $scope.Alltowelinfo1.WashClotharry[index]
                $scope.showSavebutton = true;
                for (var i = 0; i < $scope.Alltowelinfo1.WashClotharry.length; i++) {
                    if (i == index)
                        $scope.Alltowelinfo1.WashClotharry[i].HideButton = true;
                    else
                        $scope.Alltowelinfo1.WashClotharry[i].HideButton = false;
                }
            }
            $scope.showSavebutton = true;
            $scope.index4 = index;
            $scope.Grand_Total_kg3 = $scope.Grand_Total_kg3 - $scope.Alltowelinfo1.WashClotharry[$scope.index4].TotalKg3;
            $scope.Grand_Total_pc3 = $scope.Grand_Total_pc3 - $scope.Alltowelinfo1.WashClotharry[$scope.index4].Pricepc3;
        }
        catch (ex) {
            $scope.alert('Alert !!', ex, 'btn-danger', 'red');
            console.log(ex);
        }

    };

    $scope.Editdata4 = function (index) {
        try {
            if ($scope.Alltowelinfo1.WashGlovearry.length > 0) {
                $scope.Terry_Towels = $scope.Alltowelinfo1.WashGlovearry[index]
                $scope.showSavebutton = true;
                for (var i = 0; i < $scope.Alltowelinfo1.WashGlovearry.length; i++) {
                    if (i == index)
                        $scope.Alltowelinfo1.WashGlovearry[i].HideButton = true;
                    else
                        $scope.Alltowelinfo1.WashGlovearry[i].HideButton = false;
                }
            }
            $scope.showSavebutton = true;
            $scope.index5 = index;
            $scope.Grand_Total_kg4 = $scope.Grand_Total_kg4 - $scope.Alltowelinfo1.WashGlovearry[$scope.index5].TotalKg4;
            $scope.Grand_Total_pc4 = $scope.Grand_Total_pc4 - $scope.Alltowelinfo1.WashGlovearry[$scope.index5].Pricepc4;
        }
        catch (ex) {
            $scope.alert('Alert !!', ex, 'btn-danger', 'red');
            console.log(ex);
        }

    };

    $scope.Update1 = function () {
        $scope.Alltowelinfo1.BathTowelArry[$scope.index2] = $scope.Terry_Towels;
        $scope.Grand_Total_kg1 = $scope.Grand_Total_kg1 + $scope.Terry_Towels.TotalKg1;
        $scope.Grand_Total_pc1 = $scope.Grand_Total_pc1 + $scope.Terry_Towels.Pricepc1;

        $scope.Terry_Towels = {};

    }

    $scope.Update = function () {
        $scope.Alltowelinfo1.HandTowelArry[$scope.index3] = $scope.Terry_Towels;
        $scope.Grand_Total_kg2 = $scope.Grand_Total_kg2 + $scope.Terry_Towels.TotalKg2;
        $scope.Grand_Total_pc2 = $scope.Grand_Total_pc2 + $scope.Terry_Towels.Pricepc2;
        $scope.Terry_Towels = {};

    }



    $scope.Update3 = function () {
        $scope.Alltowelinfo1.WashClotharry[$scope.index4] = $scope.Terry_Towels;
        $scope.Grand_Total_kg3 = $scope.Grand_Total_kg3 + $scope.Terry_Towels.TotalKg3;
        $scope.Grand_Total_pc3 = $scope.Grand_Total_pc3 + $scope.Terry_Towels.Pricepc3;
        $scope.Terry_Towels = {};

    }

    $scope.Update4 = function () {
        $scope.Alltowelinfo1.WashGlovearry[$scope.index5] = $scope.Terry_Towels;
        $scope.Grand_Total_kg4 = $scope.Grand_Total_kg4 + $scope.Terry_Towels.TotalKg4;
        $scope.Grand_Total_pc4 = $scope.Grand_Total_pc4 + $scope.Terry_Towels.Pricepc4;
        $scope.Terry_Towels = {};

    }



    $scope.Deletemain1 = function (res) {
        $scope.Grand_Total_kg1 = $scope.Grand_Total_kg1 - $scope.Alltowelinfo1.BathTowelArry[res].TotalKg1;
        $scope.Grand_Total_pc1 = $scope.Grand_Total_pc1 - $scope.Alltowelinfo1.BathTowelArry[res].Pricepc1;
        $scope.Alltowelinfo1.BathTowelArry.splice(res, 1);
    }

    $scope.Deletemain2 = function (res) {
        $scope.Grand_Total_kg2 = $scope.Grand_Total_kg2 - $scope.Alltowelinfo1.HandTowelArry[res].TotalKg2;
        $scope.Grand_Total_pc2 = $scope.Grand_Total_pc2 - $scope.Alltowelinfo1.HandTowelArry[res].Pricepc2;
        $scope.Alltowelinfo1.HandTowelArry.splice(res, 1);
    }

    $scope.Deletemain3 = function (res) {
        $scope.Grand_Total_kg3 = $scope.Grand_Total_kg3 - $scope.Alltowelinfo1.WashClotharry[res].TotalKg3;
        $scope.Grand_Total_pc3 = $scope.Grand_Total_pc3 - $scope.Alltowelinfo1.WashClotharry[res].Pricepc3;
        $scope.Alltowelinfo1.WashClotharry.splice(res, 1);
    }

    $scope.Deletemain4 = function (res) {
        $scope.Grand_Total_kg4 = $scope.Grand_Total_kg4 - $scope.Alltowelinfo1.WashGlovearry[res].TotalKg4;
        $scope.Grand_Total_pc4 = $scope.Grand_Total_pc4 - $scope.Alltowelinfo1.WashGlovearry[res].Pricepc4;
        $scope.Alltowelinfo1.WashGlovearry.splice(res, 1);
    }

    $scope.AddValidate = function () {
        if ($scope.Terry_Towels.Particulars != undefined && $scope.Terry_Towels.Length != undefined && $scope.Terry_Towels.Width != undefined && $scope.Terry_Towels.GSMlbsdoz != undefined
        )
            return true;
        else
            return false;
    };

    $scope.AddValidate1 = function () {
        if ($scope.Terry_Towels.Length1 != undefined && $scope.Terry_Towels.Width1 != undefined && $scope.Terry_Towels.GSMlbsdoz1 != undefined
        )
            return true;
        else
            return false;
    };

    $scope.AddValidate2 = function () {
        if ($scope.Terry_Towels.Length2 != undefined && $scope.Terry_Towels.Width2 != undefined && $scope.Terry_Towels.GSMlbsdoz2 != undefined
        )
            return true;
        else
            return false;
    };

    $scope.AddValidate3 = function () {
        if ($scope.Terry_Towels.Length3 != undefined && $scope.Terry_Towels.Width3 != undefined && $scope.Terry_Towels.GSMlbsdoz3 != undefined
        )
            return true;
        else
            return false;
    };

    $scope.AddValidate4 = function () {
        if ($scope.Terry_Towels.Length4 != undefined && $scope.Terry_Towels.Width4 != undefined && $scope.Terry_Towels.GSMlbsdoz4 != undefined
        )
            return true;
        else
            return false;
    };

    $scope.selectValue = (Particulars) => {
        if (Particulars == "Bath Sheet") {
            $scope.BathSheet = true;
            $scope.BathTowel = false;
            $scope.HandTowel = false;
            $scope.WashCloth = false;
            $scope.WashGlove = false;
            $scope.materiallist = false;
            $scope.Particularsname = "Bath Sheet";
        }
        else if (Particulars == "Bath Towel") {
            $scope.BathTowel = true;
            $scope.BathSheet = false;
            $scope.HandTowel = false;
            $scope.WashCloth = false;
            $scope.WashGlove = false;
            $scope.materiallist = false;
            $scope.Particularsname = "Bath Towel";
        }
        else if (Particulars == "Hand Towel") {
            $scope.BathSheet = false;
            $scope.BathTowel = false;
            $scope.HandTowel = true;
            $scope.WashCloth = false;
            $scope.WashGlove = false;
            $scope.materiallist = false;
            $scope.Particularsname = "Hand Towel";
        }
        else if (Particulars == "Wash Cloth") {
            $scope.BathSheet = false;
            $scope.BathTowel = false;
            $scope.HandTowel = false;
            $scope.WashCloth = true;
            $scope.WashGlove = false;
            $scope.materiallist = false;
            $scope.Particularsname = "Wash Cloth";
        }
        else if (Particulars == "Wash GLOVE") {
            $scope.BathSheet = false;
            $scope.BathTowel = false;
            $scope.HandTowel = false;
            $scope.WashCloth = false;
            $scope.WashGlove = true;
            $scope.materiallist = false;
            $scope.Particularsname = "Wash GLOVE";
        }

        else {
            $scope.BathSheet = false;
            $scope.BathTowel = false;
            $scope.HandTowel = false;
            $scope.WashCloth = false;
            $scope.WashGlove = false;
            $scope.materiallist = true;
            $scope.Particularsname = "";
        }
    }


    ////////////////Row Matrials SFG Items On selection 
    $scope.x = {};
    $scope.ItemsfromGrp = [];
    $scope.ItemsfromGrp2 = [];
    $scope.ItemsfromGrpthread = [];
    $scope.ItemsfromGrpBRDPOLY = [];
    $scope.ItemsfromGrpPDQ = [];
    $scope.ItemsTable = [];
    $scope.openModal_DC = function (gcd, C) {
        $scope.Itemmodel = false;
        if ($scope.ItemsfromGrp.length == 0 && C == "11") {
            $scope.Assumstype = "11"
            $scope.ItemsforItemgrps(gcd, C);
        }
        if (C == "11" && $scope.ItemsfromGrp.length != 0) {
            $scope.Assumstype = "11"
            $scope.ItemsTable = new NgTableParams({}, { dataset: $scope.ItemsfromGrp })
        }
        if (C == "5" && $scope.ItemsfromGrp2.length != 0) {
            $scope.Assumstype = "5"
            $scope.ItemsTable = new NgTableParams({}, { dataset: $scope.ItemsfromGrp2 })
        }
        if ($scope.ItemsfromGrp2.length == 0 && C == "5") {
            $scope.Assumstype = "5"
            $scope.ItemsforItemgrps(gcd, C);
        }
        if (C == "9" && $scope.ItemsfromGrp2.length != 0) {
            $scope.Assumstype = "9"
            $scope.ItemsTable = new NgTableParams({}, { dataset: $scope.ItemsfromGrpthread })
        }
        if ($scope.ItemsfromGrpthread.length == 0 && C == "9") {
            $scope.Assumstype = "9"
            $scope.ItemsforItemgrps(gcd, C);
        }
        if (C == "12" && $scope.ItemsfromGrpBRDPOLY.length != 0) {
            $scope.Assumstype = "12"
            $scope.ItemsTable = new NgTableParams({}, { dataset: $scope.ItemsfromGrpBRDPOLY })
        }
        if ($scope.ItemsfromGrpBRDPOLY.length == 0 && C == "12") {
            $scope.Assumstype = "12"
            $scope.ItemsforItemgrps(gcd, C);
        }
        if (C == "13" && $scope.ItemsfromGrpPDQ.length != 0) {
            $scope.Assumstype = "13"
            $scope.ItemsTable = new NgTableParams({}, { dataset: $scope.ItemsfromGrpPDQ })
        }
        if ($scope.ItemsfromGrpPDQ.length == 0 && C == "13") {
            $scope.Assumstype = "13"
            $scope.ItemsforItemgrps(gcd, C);
        }

        var modal = document.getElementById('myModalDC');
        var T_Name = document.getElementById('myModalDC').value;
        modal.style.display = 'block';
    }
    $scope.closeModal_DC = function () {

        var modal = document.getElementById('myModalDC');
        modal.style.display = 'none';

    }
    $scope.SalesSETvali = 0;
    $scope.openModel_Sales = function () {
        $scope.Terry_Towels.SALExchnge = $scope.General_Inputs.Current_Exchange_Rate_val;
        if ($scope.Alltowelinfo1.SAL_Detail.length == 0) {
            $scope.SAL_Total_Price = 0;
        }
        $scope.Mat[27].Remarks = 'SET selected by User In Sales as per Invoice';
        $scope.HideModal_Sales = false;
        $scope.SalesSETvali = 1;
        var modal = document.getElementById('MyModelSales');
        var T_Name = document.getElementById('MyModelSales').value;
        modal.style.display = 'block';

    }
    $scope.closeModal_Sales = function () {
        var modal = document.getElementById('MyModelSales');
        modal.style.display = 'none';
    }
    $scope.SalesPCSvali = 0;
    $scope.openModel_Sales_PCS = function () {
        if ($scope.Alltowelinfo1.SAL_Detail.length == 0) {
            $scope.Mat[27].Remarks = 'PCS selected by User In Sales as per Invoice';
            $scope.HideModal_Sales_PSC = false;
            $scope.SalesPCSvali = 1;
            var modal = document.getElementById('MyModelSales_PSC');
            var T_Name = document.getElementById('MyModelSales_PSC').value;
            modal.style.display = 'block';
        } else {
            $scope.alert('Alert !!', 'You Cannot fill both Set and Pices !!', 'btn-danger', 'red');

        }
    }
    /////
    $scope.$watch('SAL_Total_Price', function (newVal) {
        $scope.Mat[27].Material_Value = newVal;
    });
    //working hare 
    $scope.closeModal_Sales_PCS = function () {
        if ($scope.PCSType == "KGS" || $scope.PCSType == "PCS") {
            if ($scope.PCSType == "KGS") {
                $scope.SAL_Total_Price = 0;
                $scope.z = 0;
                if ($scope.General_Inputs.Currency_Code != 'INR') {
                    for (var i = 0; i < $scope.Alltowelinfo1.BathSheetArry.length; i++) {
                        $scope.SAL_Total_Price = $scope.SAL_Total_Price + ($scope.Alltowelinfo1.BathSheetArry[i].TotalKg * ($scope.Alltowelinfo1.BathSheetArry[i].PriceKgs * $scope.General_Inputs.Current_Exchange_Rate_val))
                        if ($scope.General_Inputs.Commission_per != null)
                            $scope.Mat[25].Material_Value = parseFloat((($scope.SAL_Total_Price / 100) * $scope.General_Inputs.Commission_per).toFixed(2));

                    }
                    //$scope.SAL_Total_Price = parseFloat(($scope.SAL_Total_Price * $scope.General_Inputs.Current_Exchange_Rate_val).toFixed(2));
                    if ($scope.General_Inputs.Commission_per != null)
                        $scope.Mat[25].Material_Value = parseFloat((($scope.SAL_Total_Price * $scope.General_Inputs.Commission_per) / 100).toFixed(2));
                } else {
                    for (var i = 0; i < $scope.Alltowelinfo1.BathSheetArry.length; i++) {
                        $scope.SAL_Total_Price = $scope.SAL_Total_Price + ($scope.Alltowelinfo1.BathSheetArry[i].TotalKg * $scope.Alltowelinfo1.BathSheetArry[i].PriceKgs)
                    }
                    if ($scope.General_Inputs.Commission_per != null)
                        $scope.Mat[25].Material_Value = parseFloat((($scope.SAL_Total_Price * $scope.General_Inputs.Commission_per) / 100).toFixed(2));
                }
            } else if ($scope.PCSType == "PCS") {
                $scope.SAL_Total_Price = 0;
                $scope.z = 0;
                if ($scope.General_Inputs.Currency_Code != 'INR') {
                    for (var i = 0; i < $scope.Alltowelinfo1.BathSheetArry.length; i++) {
                        $scope.SAL_Total_Price = $scope.SAL_Total_Price + ($scope.Alltowelinfo1.BathSheetArry[i].Qty * ($scope.Alltowelinfo1.BathSheetArry[i].Pricepc * $scope.General_Inputs.Current_Exchange_Rate_val))
                        if ($scope.General_Inputs.Commission_per != null)
                            $scope.Mat[25].Material_Value = parseFloat((($scope.SAL_Total_Price / 100) * $scope.General_Inputs.Commission_per).toFixed(2));

                    }
                    //$scope.SAL_Total_Price = parseFloat(($scope.SAL_Total_Price * $scope.General_Inputs.Current_Exchange_Rate_val).toFixed(2));
                    if ($scope.General_Inputs.Commission_per != null)
                        $scope.Mat[25].Material_Value = parseFloat((($scope.SAL_Total_Price * $scope.General_Inputs.Commission_per) / 100).toFixed(2));
                } else {
                    for (var i = 0; i < $scope.Alltowelinfo1.BathSheetArry.length; i++) {
                        $scope.SAL_Total_Price = $scope.SAL_Total_Price + ($scope.Alltowelinfo1.BathSheetArry[i].Qty * $scope.Alltowelinfo1.BathSheetArry[i].Pricepc)
                    }
                    if ($scope.General_Inputs.Commission_per != null)
                        $scope.Mat[25].Material_Value = parseFloat((($scope.SAL_Total_Price * $scope.General_Inputs.Commission_per) / 100).toFixed(2));
                }
            }

            var modal = document.getElementById('MyModelSales_PSC');
            modal.style.display = 'none';
        }
        else {
            $scope.alert('Alert !!', 'Please select any of PCS Or KGS', 'btn-danger', 'red');
        }

        //$scope.ProfiteLossCalculate();
    }

    $scope.TotalSALprc = function () {
        $scope.Terry_Towels.SALTotalprice = parseFloat((($scope.Terry_Towels.SALPriceindoll * $scope.Terry_Towels.SALExchnge) * $scope.Terry_Towels.SALPcWeight).toFixed(2));
    }

    $scope.SALADD = function (i) {
        if ($scope.SAL_Validate()) {
            $scope.Alltowelinfo1.SAL_Detail.push($scope.Terry_Towels);
            if ($scope.Alltowelinfo1.SAL_Detail.length > 0) {
                $scope.Terry_Towels = {};
                $scope.Terry_Towels.SALExchnge = $scope.General_Inputs.Current_Exchange_Rate_val;
            }
        }
        else {
            $scope.alert('Alert !!', 'Please Fill Required Fields', 'btn-danger', 'red');
        }
        $scope.SAL_Total_Price = 0;
        for (var i = 0; i < $scope.Alltowelinfo1.SAL_Detail.length; i++) {
            $scope.SAL_Total_Price = $scope.SAL_Total_Price + $scope.Alltowelinfo1.SAL_Detail[i].SALTotalprice;
        }
    }
    $scope.SAL_Dlt = function (res) {
        $scope.SAL_Total_Price = parseFloat(($scope.SAL_Total_Price - ($scope.Alltowelinfo1.SAL_Detail[res].SALTotalprice)).toFixed(2));
        $scope.Alltowelinfo1.SAL_Detail.splice(res, 1);
        if ($scope.Alltowelinfo1.SAL_Detail.length == 0) {
            $scope.SAL_UDT_BTN = true;
        }
    }
    $scope.SAL_Edt = function (index) {

        $scope.Terry_Towels = $scope.Alltowelinfo1.SAL_Detail[index]
        $scope.SAL_Total_Price = parseFloat(($scope.SAL_Total_Price - ($scope.Alltowelinfo1.SAL_Detail[index].SALTotalprice)).toFixed(2));
        $scope.SAL_INDX_UP = index;
        $scope.SAL_ADD_BTN = 1;
        $scope.SAL_UDT_BTN = false;
        $scope.SAL_DLT_BTN = 1;
    }
    $scope.SAL_Updt = function () {
        $scope.Alltowelinfo1.SAL_Detail[$scope.SAL_INDX_UP] = $scope.Terry_Towels;
        $scope.SAL_Total_Price = $scope.SAL_Total_Price + $scope.Terry_Towels.SALTotalprice;
        $scope.Terry_Towels = {};
        $scope.SAL_ADD_BTN = 0;
        $scope.SAL_UDT_BTN = true;
        $scope.SAL_DLT_BTN = 0;

    }
    $scope.SAL_Validate = function () {
        if ($scope.Terry_Towels.SALPcWeight != undefined && $scope.Terry_Towels.SALPriceindoll != undefined && $scope.Terry_Towels.SALExchnge != undefined
        )
            return true;
        else
            return false;
    };
    $scope.ItemsforItemgrps = function (gcd, C) {
        try {
            $http.get('/Home/Dyes_Chem_Items?a=' + gcd).then(function (response) {
                if (response.data.Itm.length != 0) {
                    if ($scope.ItemsfromGrp.length == 0 && C == "11") {
                        $scope.ItemsfromGrp = response.data.Itm

                        $scope.ItemsTable = new NgTableParams({}, { dataset: $scope.ItemsfromGrp })
                    }
                    if ($scope.ItemsfromGrp2.length == 0 && C == "5") {
                        $scope.ItemsfromGrp2 = response.data.Itm

                        $scope.ItemsTable = new NgTableParams({}, { dataset: $scope.ItemsfromGrp2 })
                    }
                    if ($scope.ItemsfromGrpthread.length == 0 && C == "9") {
                        $scope.ItemsfromGrpthread = response.data.Itm

                        $scope.ItemsTable = new NgTableParams({}, { dataset: $scope.ItemsfromGrpthread })
                    }
                    if ($scope.ItemsfromGrpBRDPOLY.length == 0 && C == "12") {
                        $scope.ItemsfromGrpBRDPOLY = response.data.Itm

                        $scope.ItemsTable = new NgTableParams({}, { dataset: $scope.ItemsfromGrpBRDPOLY })
                    }
                    if ($scope.ItemsfromGrpPDQ.length == 0 && C == "13") {
                        $scope.ItemsfromGrpPDQ = response.data.Itm

                        $scope.ItemsTable = new NgTableParams({}, { dataset: $scope.ItemsfromGrpPDQ })
                    }

                }
            })
        } catch (ex) {
            $scope.HideLoader();
            $scope.alert('Alert !!', ex, 'btn-danger', 'red');
            console.log(ex);
        }

    }
    $scope.DsngTotVali = 0;
    $scope.GrandTotal_Cal = function (T) {
        $scope.Mat[28].Material_Value = 0;
        for (var a = 0; a < $scope.Mat.length; a++) {
            if ($scope.Mat[a].Material_Code != '29' && $scope.Mat[a].Material_Code != '28' && $scope.Mat[a].Material_Code != '30') {
                // Convert Material_Value to float and add to $scope.Mat[28].Material_Value
                var materialValue = parseFloat($scope.Mat[a].Material_Value);
                if (!isNaN(materialValue)) {
                    // Update Material_Value of index 28 with the new total
                    $scope.Mat[28].Material_Value = parseFloat(($scope.Mat[28].Material_Value + materialValue).toFixed(2));

                } else {
                    console.log('Invalid Material_Value at index ' + a + ': ' + $scope.Mat[a].Material_Value);
                }

            }

        }
        $scope.DsngTotVali = 1;
        $scope.ProfiteLossCalculate();
    }
    $scope.ItemTotCal = function (x, y) {
        x.ItemTot = x.ItmQty * x.ItmPcs;

        // Check if ItemCode already exists in $scope.DyeschemItemArray

        if (y === '11') {
            $scope.T.CortenPolybag_VALL = 0;
            var existingIndex = $scope.DyeschemItemArray.findIndex(function (item) {
                return item.ItemCode === x.ItemCode;
            });

            if (existingIndex !== -1) {
                // If ItemCode exists, update the existing entry
                $scope.DyeschemItemArray[existingIndex] = x;
            } else {
                // If ItemCode does not exist, push the new item
                $scope.DyeschemItemArray.push(x);
            }
            for (var i = 0; i < $scope.DyeschemItemArray.length; i++) {
                $scope.T.CortenPolybag_VALL = parseFloat(($scope.T.CortenPolybag_VALL + $scope.DyeschemItemArray[i].ItemTot).toFixed(2));
                $scope.Mat[18].Material_Value = $scope.T.CortenPolybag_VALL
            }
        }
        if (y === '5') {
            $scope.T.WavesChrg = 0;
            var existingIndexPT = $scope.PckingTItemArray.findIndex(function (item) {
                return item.ItemCode === x.ItemCode;
            });

            if (existingIndexPT !== -1) {
                // If ItemCode exists, update the existing entry
                $scope.PckingTItemArray[existingIndexPT] = x;
            } else {
                // If ItemCode does not exist, push the new item
                $scope.PckingTItemArray.push(x);
            }
            for (var i = 0; i < $scope.PckingTItemArray.length; i++) {
                $scope.T.WavesChrg = parseFloat(($scope.T.WavesChrg + $scope.PckingTItemArray[i].ItemTot).toFixed(2));
                $scope.Mat[10].Material_Value = $scope.T.WavesChrg;
            }
        }
        if (y === '9') {
            $scope.T.stichingthreadChrg = 0;
            var existingIndexST = $scope.StechingthreadItemArray.findIndex(function (item) {
                return item.ItemCode === x.ItemCode;
            });

            if (existingIndexST !== -1) {
                // If ItemCode exists, update the existing entry
                $scope.StechingthreadItemArray[existingIndexST] = x;
            } else {
                // If ItemCode does not exist, push the new item
                $scope.StechingthreadItemArray.push(x);
            }
            for (var i = 0; i < $scope.StechingthreadItemArray.length; i++) {
                $scope.T.stichingthreadChrg = parseFloat(($scope.T.stichingthreadChrg + $scope.StechingthreadItemArray[i].ItemTot).toFixed(2));
                $scope.Mat[9].Material_Value = $scope.T.stichingthreadChrg;
            }
        }
        if (y === '12') {
            $scope.T.Border_Polyy_VALL = 0;
            var existingIndexST = $scope.BordenPollyshtItemArray.findIndex(function (item) {
                return item.ItemCode === x.ItemCode;
            });

            if (existingIndexST !== -1) {
                // If ItemCode exists, update the existing entry
                $scope.BordenPollyshtItemArray[existingIndexST] = x;
            } else {
                // If ItemCode does not exist, push the new item
                $scope.BordenPollyshtItemArray.push(x);
            }
            for (var i = 0; i < $scope.BordenPollyshtItemArray.length; i++) {
                $scope.T.Border_Polyy_VALL = parseFloat(($scope.T.Border_Polyy_VALL + $scope.BordenPollyshtItemArray[i].ItemTot).toFixed(2));
                $scope.Mat[19].Material_Value = $scope.T.Border_Polyy_VALL;
            }
        }
        if (y === '13') {
            $scope.T.PDQ_VALL = 0;
            var existingIndexST = $scope.PDQItemArray.findIndex(function (item) {
                return item.ItemCode === x.ItemCode;
            });

            if (existingIndexST !== -1) {
                // If ItemCode exists, update the existing entry
                $scope.PDQItemArray[existingIndexST] = x;
            } else {
                // If ItemCode does not exist, push the new item
                $scope.PDQItemArray.push(x);
            }
            for (var i = 0; i < $scope.PDQItemArray.length; i++) {
                $scope.T.PDQ_VALL = parseFloat(($scope.T.PDQ_VALL + $scope.PDQItemArray[i].ItemTot).toFixed(2));
                $scope.Mat[20].Material_Value = $scope.T.PDQ_VALL;
            }
        }

    };

    //////Profite Loss Calculate 
    $scope.ProfiteLossCalculate = function () {
        $scope.Profiteval = 0;
        $scope.ProfitRess = "";
        $scope.Profiteval = parseFloat(($scope.SAL_Total_Price - $scope.Mat[28].Material_Value).toFixed(2));
        $scope.Mat[29].Material_Value = $scope.Profiteval;
        var per = parseFloat((($scope.Profiteval / $scope.Mat[28].Material_Value) * 100).toFixed(2));
        $scope.Profiteper = parseFloat((per).toFixed(2));
        $scope.Mat[29].Unit_Price = $scope.Profiteper;
        if ($scope.Profiteper < 0) {
            $scope.ProfitRess = "Costing Loss Percent(%) is " + $scope.Profiteper + "and Value of Total Loss Is " + $scope.Profiteval;
            $scope.Mat[29].Remarks = $scope.ProfitRess;
        }
        else if ($scope.Profiteper > 0) {
            $scope.ProfitRess = "Costing Profit Percent(%) is " + $scope.Profiteper + "and Value of Total Profit Is " + $scope.Profiteval;
            $scope.Mat[29].Remarks = $scope.ProfitRess;
        }
        else {
            $scope.ProfitRess = "Costing Percent(%) is " + $scope.Profiteper + "and Value Is " + $scope.Profiteval;
            $scope.Mat[29].Remarks = $scope.ProfitRess;
        }
    }
    //calculation
    $scope.CalcWeight = () => {
        $scope.PcWeight_ = ((parseFloat($scope.Terry_Towels.Length) * parseFloat($scope.Terry_Towels.Width) * parseFloat($scope.Terry_Towels.GSMlbsdoz)) / 10000);
        $scope.PcWeight_ = parseFloat($scope.PcWeight_).toFixed(2);
        $scope.PcWeight__ = parseFloat($scope.PcWeight_);
        $scope.Terry_Towels.PcWeight = $scope.PcWeight__;
    }


    $scope.CalcWeight1 = () => {
        $scope.PcWeight1_ = ((parseFloat($scope.Terry_Towels.Length1) * parseFloat($scope.Terry_Towels.Width1) * parseFloat($scope.Terry_Towels.GSMlbsdoz1)) / 10000);
        $scope.PcWeight1_ = parseFloat($scope.PcWeight1_).toFixed(3);
        $scope.PcWeight1__ = parseFloat($scope.PcWeight1_);
        $scope.Terry_Towels.PcWeight1 = $scope.PcWeight1__;
    }

    $scope.CalcWeight2 = () => {
        $scope.PcWeight2_ = ((parseFloat($scope.Terry_Towels.Length2) * parseFloat($scope.Terry_Towels.Width2) * parseFloat($scope.Terry_Towels.GSMlbsdoz2)) / 10000);
        $scope.PcWeight2_ = parseFloat($scope.PcWeight2_).toFixed(3);
        $scope.PcWeight2__ = parseFloat($scope.PcWeight2_);
        $scope.Terry_Towels.PcWeight2 = $scope.PcWeight2__;
    }

    $scope.CalcWeight3 = () => {
        $scope.PcWeight3_ = ((parseFloat($scope.Terry_Towels.Length3) * parseFloat($scope.Terry_Towels.Width3) * parseFloat($scope.Terry_Towels.GSMlbsdoz3)) / 10000);
        $scope.PcWeight3_ = parseFloat($scope.PcWeight3_).toFixed(3);
        $scope.PcWeight3__ = parseFloat($scope.PcWeight3_);
        $scope.Terry_Towels.PcWeight3 = $scope.PcWeight3__;
    }

    $scope.CalcWeight4 = () => {
        $scope.PcWeight4_ = ((parseFloat($scope.Terry_Towels.Length4) * parseFloat($scope.Terry_Towels.Width4) * parseFloat($scope.Terry_Towels.GSMlbsdoz4)) / 10000);
        $scope.PcWeight4_ = parseFloat($scope.PcWeight4_).toFixed(3);
        $scope.PcWeight4__ = parseFloat($scope.PcWeight4_);
        $scope.Terry_Towels.PcWeight4 = $scope.PcWeight4__;
    }

    $scope.TotalKgs = () => {
        $scope.TotalKg_ = ((parseFloat($scope.Terry_Towels.PcWeight) * parseFloat($scope.Terry_Towels.Qty)) / 1000);
        $scope.TotalKg_ = parseFloat($scope.TotalKg_).toFixed(2);
        $scope.TotalKg__ = parseFloat($scope.TotalKg_);
        $scope.Terry_Towels.TotalKg = $scope.TotalKg__;
        $scope.Terry_Towels.Pricepc = parseFloat(((($scope.Terry_Towels.PcWeight * $scope.General_Inputs.Price) / 1000) + 0.15).toFixed(2));
    }
    $scope.planTotalKgs = () => {
        if ($scope.Terry_Towels.planQty >= $scope.Terry_Towels.Qty) {
            $scope.Terry_Towels.planTotalKg = parseFloat(((parseFloat($scope.Terry_Towels.GryWgt) * parseFloat($scope.Terry_Towels.planQty)) / 1000).toFixed(2));
            $scope.Orderplanper();
        } else {
            $scope.alert('Alert !!', "Plane Quantity Should be Greater than or equals to  Order Quntity", 'btn-danger', 'red');
            $scope.Terry_Towels.planQty = '';
        }
    }
    $scope.Orderplanper = function () {
        $scope.Terry_Towels.OrderplanTotalper = parseFloat(((($scope.Terry_Towels.planQty - $scope.Terry_Towels.Qty) / $scope.Terry_Towels.planQty) * 100).toFixed(2));
        //var ab = ($scope.Terry_Towels.planQty * $scope.Terry_Towels.GryWgt) / 1000;
        //$scope.Terry_Towels.TotalWaveLoss = parseFloat((ab / $scope.Terry_Towels.WaveLossper).toFixed(2));
        $scope.Terry_Towels.TotalWaveLoss = parseFloat(($scope.Terry_Towels.planTotalKg / (1 - ($scope.Terry_Towels.WaveLossper / 100))).toFixed(2));
    }
    $scope.GryWgt_Cal = function () {
        var a = (100 - $scope.Terry_Towels.DyWgt) / 100;
        $scope.Terry_Towels.GryWgt = parseFloat(($scope.Terry_Towels.PcWeight / a).toFixed(2));
    }
    $scope.TotalKgs1 = () => {
        $scope.TotalKg1_ = ((parseFloat($scope.Terry_Towels.PcWeight1) * parseFloat($scope.Terry_Towels.Qty1)) / 1000);
        $scope.TotalKg1_ = parseFloat($scope.TotalKg1_).toFixed(3);
        $scope.TotalKg1__ = parseFloat($scope.TotalKg1_);
        $scope.Terry_Towels.TotalKg1 = $scope.TotalKg1__;
    }

    $scope.TotalKgs2 = () => {
        $scope.TotalKg2_ = ((parseFloat($scope.Terry_Towels.PcWeight2) * parseFloat($scope.Terry_Towels.Qty2)) / 1000);
        $scope.TotalKg2_ = parseFloat($scope.TotalKg2_).toFixed(3);
        $scope.TotalKg2__ = parseFloat($scope.TotalKg2_);
        $scope.Terry_Towels.TotalKg2 = $scope.TotalKg2__;
    }

    $scope.TotalKgs3 = () => {
        $scope.TotalKg3_ = ((parseFloat($scope.Terry_Towels.PcWeight3) * parseFloat($scope.Terry_Towels.Qty3)) / 1000);
        $scope.TotalKg3_ = parseFloat($scope.TotalKg3_).toFixed(3);
        $scope.TotalKg3__ = parseFloat($scope.TotalKg3_);
        $scope.Terry_Towels.TotalKg3 = $scope.TotalKg3__;
    }

    $scope.TotalKgs4 = () => {
        $scope.TotalKg4_ = ((parseFloat($scope.Terry_Towels.PcWeight4) * parseFloat($scope.Terry_Towels.Qty4)) / 1000);
        $scope.TotalKg4_ = parseFloat($scope.TotalKg4_).toFixed(3);
        $scope.TotalKg4__ = parseFloat($scope.TotalKg4_);
        $scope.Terry_Towels.TotalKg4 = $scope.TotalKg4__;
    }

    $scope.ItemSelected = function (ind, d) {
        try {
            let ItemCode = $scope.RowData[ind].ItemCode;
            let DocEntry = $scope.BomData.DocEntry;
            angular.element('#loading').show();
            $http.get('/api/WebAPI/ExploreBom?ItemCode=' + ItemCode + "&DocEntry=" + DocEntry).then(function (response) {
                $scope.BomExploreData = response.data;
                //------------------------------------Logic DeliveryDate-------------------------------------------------
                var Days = 0;
                if ($scope.BomExploreData.length != 0) {
                    for (let k = 0; k < $scope.BomExploreData.length; k++) {
                        Days = Days + parseFloat($scope.BomExploreData[k].Child1Days);
                        var DeliveryDate = new Date();
                        DeliveryDate.setDate(DeliveryDate.getDate() + Days);
                        $scope.DateAfterCal = DeliveryDate;
                    }
                }
                angular.element('#loading').hide();

            }).catch(function (response) {
                $scope.alert('Alert !!', response.data, 'btn-danger', 'red');
                console.log(response.data);
            })
        }
        catch (ex) {
            $scope.HideLoader();
            $scope.alert('Alert !!', ex, 'btn-danger', 'red');
            console.log(ex);
        }
    };

    $scope.AlertFun = function (title, msg, btnclss, typeclr) {
        $ngConfirm({
            title: title,
            content: msg,
            type: typeclr,
            typeAnimated: true,
            buttons: {
                close: {
                    text: 'OK',
                    btnClass: btnclss,
                    action: function () {
                        if (title === 'Success !!')
                            location.reload();
                    }
                },
                OK: {
                    text: 'Copy To Sales Order',
                    btnClass: 'btn-success',
                    action: function () {
                        $scope.CopyQuotationToOrder();
                    }
                }
            }

        });
    };

    $scope.HideLoader = function () {
        angular.element('#loading').hide();
    };

    $scope.showloader = function () {
        angular.element('#loading').show();
    };

});

app.controller('abc', function ($rootScope, $scope, $http, NgTableParams, $ngConfirm, $q, Upload) {
    //--------------------------------------Dashboard Page------------------------
    $scope.Init = () => {
        $scope.HideLoader();
    }

    $scope.alert = function (title, msg, btnclss, typeclr) {
        $ngConfirm({
            title: title,
            content: msg,
            type: typeclr,
            typeAnimated: true,
            buttons: {
                close: {
                    text: 'OK',
                    btnClass: btnclss,
                    action: function () {
                        if (title === 'Success !!')
                            location.reload();
                    }
                }
            }
        });
    };

    $scope.AlertFun = function (title, msg, btnclss, typeclr) {
        $ngConfirm({
            title: title,
            content: msg,
            type: typeclr,
            typeAnimated: true,
            buttons: {
                close: {
                    text: 'OK',
                    btnClass: btnclss,
                    action: function () {
                        if (title === 'Success !!')
                            location.reload();
                    }
                },
                OK: {
                    text: 'Copy To Sales Order',
                    btnClass: 'btn-success',
                    action: function () {
                        $scope.CopyQuotationToOrder();
                    }
                }
            }

        });
    };

    $scope.HideLoader = function () {
        angular.element('#loading').hide();
    };

    $scope.showloader = function () {
        angular.element('#loading').show();
    };


});
app.controller('XLController', function ($rootScope, $scope, $http, NgTableParams, $ngConfirm, $q, Upload) {
    //--------------------------------------Dashboard Page------------------------
    $scope.XlInit = () => {
        $scope.HideLoader();
    }
    $scope.alert = function (title, msg, btnclss, typeclr) {
        $ngConfirm({
            title: title,
            content: msg,
            type: typeclr,
            typeAnimated: true,
            buttons: {
                close: {
                    text: 'OK',
                    btnClass: btnclss,
                    action: function () {
                        if (title === 'Success !!')
                            location.reload();
                    }
                }
            }
        });
    };

    $scope.AlertFun = function (title, msg, btnclss, typeclr) {
        $ngConfirm({
            title: title,
            content: msg,
            type: typeclr,
            typeAnimated: true,
            buttons: {
                close: {
                    text: 'OK',
                    btnClass: btnclss,
                    action: function () {
                        if (title === 'Success !!')
                            location.reload();
                    }
                },
                OK: {
                    text: 'Copy To Sales Order',
                    btnClass: 'btn-success',
                    action: function () {
                        $scope.CopyQuotationToOrder();
                    }
                }
            }

        });
    };

    $scope.HideLoader = function () {
        angular.element('#loading').hide();
    };

    $scope.showloader = function () {
        angular.element('#loading').show();
    };


});
app.controller('SaleOrder', function ($rootScope, $scope, $http, NgTableParams, $ngConfirm, $q, Upload) {
    //--------------------------------------Dashboard Page------------------------
    $scope.SaleOrderInit = function () {
        try {
            angular.element('#loading').show();
            $scope.angodiv = true;
            $scope.Costing_Item = [];
            $http.get('/Home/SaleOrderInit').then(function (response) {
                $scope.OCRD = response.data.OCRD;
                $scope.OCRD = response.data.OCRD;
                //$scope.created_costing = response.data.OCRD3;
                $scope.created_costing = new NgTableParams({}, { dataset: response.data.OCRD3 });
                $scope.WareHouse = response.data.WareHouse;
                $scope.TaxName = response.data.TaxName;
                angular.element('#loading').hide();
            }).catch(function (response) {
                $scope.alert('Alert !!', response.data, 'btn-danger', 'red');
                console.log(response.data);
            })

        }
        catch (ex) {
            $scope.HideLoader();
            $scope.alert('Alert !!', ex, 'btn-danger', 'red');
            console.log(ex);
        }
    };

    $scope.Costing_dtl = function (d) {
        $scope.DocEn = $scope.created_costing.filter(item => item.Costing_Num === $scope.Costing_Num)
        /*.map(item => item.DocEntry)*/
        var params = {
            Costing_num: d.Costing_Num,
            created_costing_DocEntry: d.DocEntry
        };
        $scope.CustomerCode = d.Code,
            $scope.CustomerName = d.CustomerName,

            $http.get('/Home/Costing_dtl', { params: params })
                .then(function (ress) {
                    if (ress.data.length === 0) {
                        $scope.alert('Alert !!', "No Costing for this '" + $scope.CustomerName + "'Customer '", 'btn-success', 'green');
                        $scope.Costing_Item = [];
                    }
                    else {
                        // Ensure ress.data.Itm and ress.data.Itm2 have the same length to avoid index out of bounds
                        if (ress.data.Itm.length === ress.data.Itm2.length) {
                            for (var i = 0; i < ress.data.Itm.length; i++) {
                                // Merge data from ress.data.Itm2 into ress.data.Itm
                                ress.data.Itm[i].CostingNum = ress.data.Itm2[i].CostingNum;
                                ress.data.Itm[i].DocEntry = ress.data.Itm2[i].DocEntry;
                                ress.data.Itm[i].OrderQty = ress.data.Itm2[i].OrderQty;
                                ress.data.Itm[i].Priceperpc = ress.data.Itm2[i].Priceperpc;
                                ress.data.Itm[i].Totalkg = ress.data.Itm2[i].Totalkg;
                                ress.data.Itm[i].U_merchandiserCode = ress.data.Itm2[i].U_merchandiserCode;
                                ress.data.Itm[i].U_Exchangerate = ress.data.Itm2[i].U_Exchangerate;
                                ress.data.Itm[i].U_ExchangeCode = ress.data.Itm2[i].U_ExchangeCode;
                                ress.data.Itm[i].SaleOrd_No = ress.data.Itm2[i].SaleOrd_No;
                                ress.data.Itm[i].CRN = ress.data.Itm2[i].CRN;
                                ress.data.Itm[i].TotalOrderQty = ress.data.Itm2[i].TotalOrderQty;
                                ress.data.Itm[i].Grand_Total = parseFloat((ress.data.Itm2[i].Priceperpc * ress.data.Itm2[i].OrderQty).toFixed(2));
                            }
                            if ($scope.Costing_Item.length != 0) {
                                $scope.Costing_Item = [];
                            }
                            $scope.Costing_Item.push(...ress.data.Itm);
                            $scope.abc();
                        } else {
                            console.error('Data length mismatch between ress.data.Itm and ress.data.Itm2');
                            $scope.Costing_Item = [];
                            $scope.alert('Alert !!', "Costing Data Not Valid Because Number of Item Not Match with UDO and Item master !!'", 'btn-danger', 'red');
                        }
                    }

                });
    };
    $scope.SL_Statefind = function () {
        try {
            $http.get('/Home/SL_Statefind', { params: { StCode: $scope.Ship_To_v } }).then(function (ress) {
                if (ress.data != null) {
                    $scope.Stname = ress.data;
                }
                else {

                }
            });
        }
        catch (ex) {
            console(ex);
        }
    }
    $scope.abc = function () {
        try {
            $http.get('/Home/Bill_to_Ship_to', { params: { Code: $scope.CustomerCode } }).then(function (ress) {
                if (ress.data.OCRD2 != null) {
                    $scope.B_S_Data = ress.data.OCRD;
                    $scope.B_S_Data2 = ress.data.OCRD2;
                }
            });
        }
        catch (ex) {
            console("Name not Found");
        }
    }
    $scope.Total_Sales_cal = function (item) {
        item.Total_Sales_p = parseFloat(($scope.Costing_GT * item.Total_for_Sales).toFixed(2));
    }

    $scope.dtlitem = function (i) {
        $scope.Costing_Item.splice(i, 1);
    }

    $scope.SaleOrderPunch = function () {
        angular.element('#loading').show();
        $scope.sale_order_data = {}
        $scope.sale_order_data.CustomerName = $scope.CustomerName;
        $scope.sale_order_data.CustomerCode = $scope.CustomerCode;
        $scope.sale_order_data.Bill_To_v = $scope.Bill_To_v;
        $scope.sale_order_data.Ship_To_v = $scope.Ship_To_v;
        $scope.sale_order_data.Post_Date = moment($scope.Post_Date).format("DD-MM-YYYY");
        $scope.sale_order_data.Deliviry_Date = moment($scope.Deliviry_Date).format("DD-MM-YYYY");
        $scope.sale_order_data.CostingNum = $scope.CostingNum;
        $scope.sale_order_data.ItemDlt = $scope.Costing_Item;
        $http.post('/Home/SaleOrderPunch', $scope.sale_order_data).then(function (response) {
            angular.element('#loading').hide();
            if (response.data == "ok") {
                $scope.alert('Alert !!', "Sales Order Punched !!", 'btn-success', 'green');
                window.addEventListener('click', function () {
                    location.reload();
                });
            } else if (response.data == "POPunch") {
                $scope.alert('Alert !!', "Sales Order Punched But Production Order Not Done !!", 'btn-success', 'green');
            }
            else {
                $scope.alert('Alert !!', response.data, 'btn-danger', 'red');
            }
        })

    }

    $scope.alert = function (title, msg, btnclss, typeclr) {
        $ngConfirm({
            title: title,
            content: msg,
            type: typeclr,
            typeAnimated: true,
            buttons: {
                close: {
                    text: 'OK',
                    btnClass: btnclss,
                    action: function () {
                        if (title === 'Success !!')
                            location.reload();
                    }
                }
            }
        });
    };

    $scope.AlertFun = function (title, msg, btnclss, typeclr) {
        $ngConfirm({
            title: title,
            content: msg,
            type: typeclr,
            typeAnimated: true,
            buttons: {
                close: {
                    text: 'OK',
                    btnClass: btnclss,
                    action: function () {
                        if (title === 'Success !!')
                            location.reload();
                    }
                },
                OK: {
                    text: 'Copy To Sales Order',
                    btnClass: 'btn-success',
                    action: function () {
                        $scope.CopyQuotationToOrder();
                    }
                }
            }

        });
    };

    $scope.HideLoader = function () {
        angular.element('#loading').hide();
    };

    $scope.showloader = function () {
        angular.element('#loading').show();
    };


});

app.controller('ReportCtrl', function ($rootScope, $scope, $http, NgTableParams, $ngConfirm, $q, Upload) {

    $scope.ViewCostingData = function () {
        try {
            angular.element('#loading').show();
            $http.get("/Home/FindCustomer").then(function (res) {

                if (res != null) {
                    $scope.Alltowelinfo1 = res.data.abc;
                    $scope.AllCostingData = new NgTableParams({}, { dataset: res.data.abc });
                    angular.element('#loading').hide();
                    //$scope.alert('', "Find Success", 'btn-success', 'green', 'close');
                }
                if (res == null) {
                    $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
                }
            });

        }
        catch (ex) {
            console.log(ex);
        }

    }
    $scope.ReportCtrl = function (CostingNumSb) {
        $http.get('/Home/ReportCtrl?CostingNum=' + CostingNumSb).then(function (res) {

            if (res != null) {
                $scope.Alltowelinfo1 = res.data.abc;
                $scope.AllCostingData = new NgTableParams({}, { dataset: res.data.abc });
                angular.element('#loading').hide();
                //$scope.alert('', "Find Success", 'btn-success', 'green', 'close');
            }
            if (res == null) {
                $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
            }
        });
        //$http.get('/Home/ReportCtrl?CostingNum='+ CostingNumSb).then(function (res) {

        // });

    }
    $scope.alert = function (title, msg, btnclss, typeclr) {
        $ngConfirm({
            title: title,
            content: msg,
            type: typeclr,
            typeAnimated: true,
            buttons: {
                close: {
                    text: 'OK',
                    btnClass: btnclss,
                    action: function () {
                        if (title === 'Success !!')
                            location.reload();
                    }
                }
            }
        });
    };

    $scope.AlertFun = function (title, msg, btnclss, typeclr) {
        $ngConfirm({
            title: title,
            content: msg,
            type: typeclr,
            typeAnimated: true,
            buttons: {
                close: {
                    text: 'OK',
                    btnClass: btnclss,
                    action: function () {
                        if (title === 'Success !!')
                            location.reload();
                    }
                },
                OK: {
                    text: 'Copy To Sales Order',
                    btnClass: 'btn-success',
                    action: function () {
                        $scope.CopyQuotationToOrder();
                    }
                }
            }

        });
    };

    $scope.HideLoader = function () {
        angular.element('#loading').hide();
    };

    $scope.showloader = function () {
        angular.element('#loading').show();
    };


});

app.controller('DashboardPage', function ($rootScope, $scope, $http, NgTableParams, $ngConfirm, $q, Upload) {
    $scope.DashInit = function () {
        try {
            angular.element('#loading').show();
            $http.get('/api/WebAPI/TeryTowelInit').then(function (response) {
                angular.element('#loading').hide();
                $scope.Total_Costing = response.data.counts[0];
                $scope.Total_Ap_Costing = response.data.counts[1];
                $scope.Total_Rj_Costing = response.data.counts[2];
                $scope.Total_Pn_Costing = response.data.counts[3];
            });
        }
        catch (ex) {
            $scope.HideLoader();
            $scope.alert('Alert !!', ex, 'btn-danger', 'red');
            console.log(ex);
        }
    }
    $scope.ShowStatus = function (x) {
        try {
            $http.get('/Home/ShowStatus?docentry=' + x.docentry).then(function (res) {
                if (res.Message == undefined) {
                    $scope.AuditStatus = new NgTableParams({}, { dataset: res.data.AuditStatus });
                }
                else {
                    $scope.alert('Alert !!', "Satatus Not Found !!", 'btn-danger', 'red');
                }
            });
        } catch (e) {
            console.log(e);
        }
        
    }
    document.addEventListener('visibilitychange', function () {
        if (document.visibilityState === 'hidden') {
            navigator.sendBeacon("/ABC/RemoveSession");
        }
    });
    window.onbeforeunload = function () {
        navigator.sendBeacon("/ABC/RemoveSession");
    }

    
    $scope.Created_Costing = function () {
        $http.get("/Home/FindCustomer").then(function (res) {
            if (res != null) {
                $scope.Alltowelinfo1 = res.data.abc;
                $scope.usersTable = new NgTableParams({}, { dataset: res.data.abc });
            }
            if (res == null) {
                $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
            }
        });
    }
    //////////////////////////////////////////////Total Approved Costing Detail////////////////////////////////////////////
    $scope.FindApprovedCosting = function (i) {

        if (i == "Approved") {
            $http.get("/Home/FindApprovedCosting", { params: { Status: i } }).then(function (res) {
                if (res.data !== null) {
                    $scope.usersTable1 = new NgTableParams({}, { dataset: res.data.abc });
                } else {
                    $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
                }
            });
        }
        else if (i == "Reject") {
            $http.get("/Home/FindApprovedCosting", { params: { Status: i } }).then(function (res) {
                if (res.data !== null) {
                    $scope.usersTable2 = new NgTableParams({}, { dataset: res.data.abc });
                } else {
                    $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
                }
            });
        }
        else {
            $http.get("/Home/FindApprovedCosting", { params: { Status: i } }).then(function (res) {
                if (res.data !== null) {
                    $scope.usersTable3 = new NgTableParams({}, { dataset: res.data.abc });
                } else {
                    $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
                }
            });
        }
    }
});

app.controller('ReportPage', function ($rootScope, $scope, $http, NgTableParams, $ngConfirm, $q, Upload) {
    $scope.ReportinitPage = function () {
        try {
            $scope.HideModal_Sales = true;
            $scope.PrintRepaort = false;
            angular.element('#loading').show();
            $http.get("/Home/FindCustomer").then(function (res) {

                if (res != null) {
                    $scope.Alltowelinfo1 = res.data.abc;
                    $scope.AllCostingData = new NgTableParams({}, { dataset: res.data.abc });
                    angular.element('#loading').hide();
                    //$scope.alert('', "Find Success", 'btn-success', 'green', 'close');
                }
                if (res == null) {
                    $scope.alert('Alert !!', "Find Not Success", 'btn-danger', 'red');
                }
            });
        }
        catch (ex) {
            console.log(ex);
        }
    }

    $scope.closeModal_Sales = function () {
        var modal = document.getElementById('Myreport');
        modal.style.display = 'none';
    }
    $scope.GetAllRecordForReport = function (docentry) {
        try {
            if (docentry != null) {

                var docentryint = parseInt(docentry);
                $scope.materiallist = false;
                $scope.Afterclickcosttype = true;
                $scope.angodiv = false;
                $scope.namediv = true;
                $scope.DocEntry_val = docentry;
                $scope.Costing_num_at_Edit = false;
                $scope.Costing_num_at_Insert = true;
                $scope.date_on_edit = false;
                $scope.date_on_Insert = true;
                $scope.RenewvalBtn = false;
                $scope.Mat = [];
                $scope.Allvalues = [];
                $scope.Alltowelinfo1 = {};
                $scope.Alltowelinfo1.BathSheetArry = [];
                $http.get('/Home/GetAllRecordForReport?docentry=' + docentryint).then(function (res) {
                    $scope.HideModal_Sales = false;
                    var modal = document.getElementById('Myreport');
                    var T_Name = document.getElementById('Myreport').value;
                    modal.style.display = 'block';
                    $scope.CustomerName = res.data.abc3[0].Customername;
                    $scope.CustomerCode = res.data.abc3[0].CustomerCode;
                    $scope.cdate = res.data.abc3[0].CostingDate;
                    $scope.Dyed_Type = res.data.abc3[0].DYED_VAL;
                    $scope.Weaving_Type = res.data.abc3[0].Weaving_VAL;
                    $scope.Costing_Num = res.data.abc3[0].CostingNum;
                    $scope.SaleNum = res.data.abc3[0].SaleNum;

                    $scope.merchandiser = res.data.abc3[0].merchandiser;
                    $scope.merchandiserCode = res.data.abc3[0].merchandiserCode;
                    $scope.CRefNo = res.data.abc3[0].CRefNo;
                    $scope.General_Inputs = res.data.GENRAL_INPUTS[0];

                    for (var i = 0; i < res.data.abc2.length; i++) {
                        costdata_ = parseFloat(res.data.abc2[i].costdata)
                        Percentage_ = parseFloat(res.data.abc2[i].Percentage)
                        Percentage_val_ = parseFloat(res.data.abc2[i].Percentage_val)
                        $scope.Allvalues.push({
                            Type: res.data.abc2[i].Type,
                            SD_VAL: res.data.abc2[i].SD_VAL,
                            countdata: res.data.abc2[i].countdata,
                            card_cmbd_OE: res.data.abc2[i].card_cmbd_OE,
                            costdata: costdata_,
                            Percentage: Percentage_,
                            Percentage_val: Percentage_val_
                        });
                    }
                    $scope.Allinfo_total();
                    for (var i = 0; i < res.data.BATHSHEETARRY.length; i++) {
                        var item = res.data.BATHSHEETARRY[i];
                        item.Length = parseFloat(item.Length) || 0;
                        item.Width = parseFloat(item.Width) || 0;
                        item.GSMlbsdoz = parseFloat(item.GSMlbsdoz) || 0;
                        item.PcWeight = parseFloat(item.PcWeight) || 0;
                        item.Qty = parseFloat(item.Qty) || 0;
                        item.TotalKg = parseFloat(item.TotalKg) || 0;
                        item.Pricepc = parseFloat(item.Pricepc) || 0;
                        item.DyWgt = parseFloat(item.DyWgt) || 0;
                        item.GryWgt = parseFloat(item.GryWgt) || 0;
                        item.WaveLossper = parseFloat(item.WaveLossper) || 0;
                        item.planQty = parseFloat(item.planQty) || 0;
                        item.planTotalKg = parseFloat(item.planTotalKg) || 0;
                        item.OrderplanTotalper = parseFloat(item.OrderplanTotalper) || 0;
                        item.PriceKgs = parseFloat(item.PriceKgs) || 0;
                        item.TotalWaveLoss = parseFloat(item.TotalWaveLoss) || 0;
                    }
                    $scope.Alltowelinfo1.BathSheetArry = res.data.BATHSHEETARRY;
                    if (res.data.BATHSHEETARRY.length != 0) {
                        $scope.AddRow_Totals();
                    }
                    $scope.Production_Process_Detail = res.data.Production_Process;
                    $scope.Mat = res.data.ASSUMPTION_DETAILS;
                    for (var i = 0; i < $scope.Mat.length; i++) {
                        $scope.Mat[i].Unit_Price = parseFloat($scope.Mat[i].Unit_Price);
                        $scope.Mat[i].Quantity = parseFloat($scope.Mat[i].Quantity);
                        $scope.Mat[i].Material_Value = parseFloat($scope.Mat[i].Material_Value);
                        if ($scope.Mat[i].Material_Code == '28') {
                            $scope.SAL_Total_Price = $scope.Mat[i].Material_Value
                        }
                    }

                    $scope.Filearr = res.data.FileDAta;
                    $scope.Commission_per = parseFloat(res.data.abc3[0].Commition);
                    $scope.Ocean_freight_val = parseFloat(res.data.abc3[0].Oceanfreightinclddc);
                    $scope.Price = parseFloat(res.data.abc3[0].Contractprice);
                    $scope.electricityval = parseFloat(res.data.abc3[0].Electricity);
                    $scope.Streamval = parseFloat(res.data.abc3[0].Stream);
                    $scope.Mktgval = parseFloat(res.data.abc3[0].MktgExpenses);
                    $scope.valuelossval = parseFloat(res.data.abc3[0].ValueLoss);
                    $scope.commitionval = parseFloat(res.data.abc3[0].Commition);
                })

            }
        }
        catch (ex) {

        }

    }
    $scope.Allinfo_total = function () {
        $scope.Percentage_total_Tinfo = 0.0;
        $scope.costdata_Percentage_total_Tinfo = 0.0;
        for (var i = 0; i < $scope.Allvalues.length; i++) {
            $scope.Percentage_total_Tinfo = parseFloat(($scope.Percentage_total_Tinfo + $scope.Allvalues[i].Percentage).toFixed(2));
            $scope.costdata_Percentage_total_Tinfo = parseFloat(($scope.costdata_Percentage_total_Tinfo + $scope.Allvalues[i].Percentage_val).toFixed(2));
        }
    }

    $scope.PrintRepaort = true;
    //Repot Print
    $scope.Print = (elem) => {
        $scope.PrintRepaort = false;

        var printElement = document.getElementById(elem);

        if (printElement) {
            var printContents = printElement.innerHTML;
            var originalContents = document.body.innerHTML;

            // Open a new window for printing
            var printWindow = window.open('', '_blank');

            // Write the HTML content to the new window
            printWindow.document.write('<html><head><title>' + document.title + '</title></head><body>');
            printWindow.document.write(printContents);
            printWindow.document.write('</body></html>');

            // Print and close the new window
            printWindow.document.close();
            printWindow.print();
            printWindow.onafterprint = function () {
                printWindow.close();
            };
            // Restore the original content to the main window
            document.body.innerHTML = originalContents;
            location.reload();
        } else {
            console.error("Element with ID '" + elem + "' not found.");
        }
    };
    $scope.Print1 = (elem) => {
        $scope.PrintRepaort = false;

        var printElement = document.getElementById(elem);

        if (printElement) {
            var printContents = printElement.innerHTML;
            var originalContents = document.body.innerHTML;

            // Open a new window for printing
            var printWindow = window.open('', '_blank');

            // Write the HTML content to the new window
            printWindow.document.write('<html><head><title>' + document.title + '</title>');
            printWindow.document.write('<style>');
            printWindow.document.write('body { margin: 20px; }');
            printWindow.document.write('.table { border-collapse: collapse; }'); // Full-width table
            printWindow.document.write('.table th, .table td {');
            printWindow.document.write('border: 2px solid #000;'); // Bold border
            printWindow.document.write('padding: 10px;'); // Padding for cells
            printWindow.document.write('text-align: left;'); // Left-align text
            printWindow.document.write('overflow: hidden;'); // Hide overflow text
            printWindow.document.write('text-overflow: ellipsis;'); // Ellipsis for overflow text
            printWindow.document.write('white-space: nowrap;'); // Prevent text wrapping
            printWindow.document.write('}');
            printWindow.document.write('.table th { background-color: #f2f2f2; }'); // Header background
            printWindow.document.write('</style>');
            printWindow.document.write('</head><body>');
            printWindow.document.write(printContents);
            printWindow.document.write('</body></html>');

            // Print and close the new window
            printWindow.document.close();
            printWindow.print();
            printWindow.onafterprint = function () {
                printWindow.close();
            };

            // Restore the original content to the main window
            document.body.innerHTML = originalContents;
            location.reload();
        } else {
            console.error("Element with ID '" + elem + "' not found.");
        }
    };



    $scope.AddRow_Totals = function () {
        // Initialize totals
        $scope.Grand_Total_kg = 0;
        $scope.Grand_Total_pc = 0;
        $scope.Grand_Total_Qty = 0;
        $scope.Grand_Total_GSM_Qty = 0;
        $scope.Grand_Total_GSM_Qty_AVG = 0;
        $scope.Wavesloss_Total = 0;
        $scope.Graywt_Total = 0;
        $scope.wtloss_Total_per = 0;
        $scope.Yarn_Total_Val = 0; // Adjusted from 1 to 0
        $scope.planQty_Total = 0;
        $scope.planQty_Total_Kg = 0;

        if (!$scope.Mat) {
            $scope.Mat = [];
        }

        for (var i = 0; i < $scope.Alltowelinfo1.BathSheetArry.length; i++) {
            var item = $scope.Alltowelinfo1.BathSheetArry[i];

            // Safely parse values
            var totalKg = parseFloat(item.TotalKg) || 0;
            var pricePc = parseFloat(item.Pricepc) || 0;
            var qty = parseFloat(item.Qty) || 0;
            var gsmLbsdoz = parseFloat(item.GSMlbsdoz) || 0;
            var totalWaveLoss = parseFloat(item.TotalWaveLoss) || 0;
            var grayWgt = parseFloat(item.GryWgt) || 0;
            var waveLossper = parseFloat(item.WaveLossper) || 0;
            var planQty = parseFloat(item.planQty) || 0;
            var planTotalKg = parseFloat(item.planTotalKg) || 0;

            // Calculate totals
            $scope.Grand_Total_kg += totalKg;
            $scope.Grand_Total_pc += pricePc;
            $scope.Grand_Total_Qty += qty;
            $scope.Grand_Total_GSM_Qty += (gsmLbsdoz * qty);
            $scope.Wavesloss_Total += parseFloat((totalWaveLoss).toFixed(2));
            $scope.Graywt_Total += grayWgt;
            $scope.wtloss_Total_per += waveLossper;
            $scope.Yarn_Total_Val = parseFloat(($scope.Wavesloss_Total * ($scope.costdata_Percentage_total_Tinfo || 0)).toFixed(2));

            // Initialize the Mat array if not already done
            if (!$scope.Mat[0]) {
                $scope.Mat[0] = {};
            }
            $scope.Mat[0].Material_Value = $scope.Yarn_Total_Val;
            $scope.Mat[0].Quantity = parseFloat(($scope.Wavesloss_Total).toFixed(2));
            $scope.Mat[0].Unit_Price = $scope.costdata_Percentage_total_Tinfo;

            // Update plan totals
            $scope.planQty_Total += planQty;
            $scope.planQty_Total_Kg += planTotalKg;
        }
        if ($scope.Alltowelinfo1.Dyed_Type == 'PIECE DYED') {
            var totalWaveLoss = parseFloat($scope.Alltowelinfo1.BathSheetArry[0].TotalWaveLoss) || 0;
            if (!$scope.Mat[1]) {
                $scope.Mat[1] = {};
            }
            if (!$scope.Mat[2]) {
                $scope.Mat[2] = {};
            }
            // Swapping the assignments
            $scope.Mat[1].Quantity = parseFloat(($scope.Mat[0].Quantity).toFixed(2));
            //$scope.Mat[1].Quantity = parseFloat((totalWaveLoss).toFixed(2));
            /*$scope.Mat[2].Quantity = parseFloat(($scope.Wavesloss_Total * ((100 - $scope.Alltowelinfo1.BathSheetArry[0].DyWgt) / 100)).toFixed(2));*/
            //$scope.Mat[2].Quantity = parseFloat(($scope.Alltowelinfo1.BathSheetArry[0].TotalWaveLoss * ((100 - $scope.Alltowelinfo1.BathSheetArry[0].DyWgt) / 100)).toFixed(2));
            $scope.Mat[2].Quantity = parseFloat(($scope.Mat[0].Quantity * ((100 - $scope.Alltowelinfo1.BathSheetArry[0].WaveLossper) / 100)).toFixed(2));
        }
        // Format the final results with toFixed
        $scope.Grand_Total_kg = parseFloat($scope.Grand_Total_kg.toFixed(2));
        $scope.Grand_Total_pc = parseFloat($scope.Grand_Total_pc.toFixed(2));
        $scope.Grand_Total_GSM_Qty = parseFloat($scope.Grand_Total_GSM_Qty.toFixed(2));
        $scope.Grand_Total_GSM_Qty_AVG = $scope.Grand_Total_Qty > 0 ? parseFloat(($scope.Grand_Total_GSM_Qty / $scope.Grand_Total_Qty).toFixed(2)) : 0;
        $scope.Wavesloss_Total = parseFloat($scope.Wavesloss_Total.toFixed(2));
        $scope.Graywt_Total = parseFloat($scope.Graywt_Total.toFixed(2));
        $scope.wtloss_Total_per = parseFloat($scope.wtloss_Total_per.toFixed(2));
        $scope.planQty_Total = parseFloat($scope.planQty_Total.toFixed(2));
        $scope.planQty_Total_Kg = parseFloat($scope.planQty_Total_Kg.toFixed(2));

        if ($scope.Alltowelinfo1.Dyed_Type == 'YARN DYED') {
            if (!$scope.Mat[1]) {
                $scope.Mat[1] = {};
            }
            if (!$scope.Mat[2]) {
                $scope.Mat[2] = {};
            }
            // Swapping the assignments
            /*$scope.Mat[1].Quantity = $scope.planQty_Total;*/
            $scope.Mat[2].Quantity = parseFloat(($scope.Mat[0].Quantity).toFixed(2));
            /*$scope.Mat[1].Quantity = parseFloat(($scope.Wavesloss_Total * ((100 - $scope.Alltowelinfo1.BathSheetArry[0].DyWgt) / 100)).toFixed(2));*/
            $scope.Mat[1].Quantity = parseFloat(($scope.Mat[0].Quantity * ((100 - $scope.Alltowelinfo1.BathSheetArry[0].DyWgt) / 100)).toFixed(2));
        }
    }
    $scope.alert = function (title, msg, btnclss, typeclr) {
        $ngConfirm({
            title: title,
            content: msg,
            type: typeclr,
            typeAnimated: true,
            buttons: {
                close: {
                    text: 'OK',
                    btnClass: btnclss,
                    action: function () {
                        if (title === 'Success !!')
                            location.reload();
                    }
                }
            }
        });
    };

    $scope.AlertFun = function (title, msg, btnclss, typeclr) {
        $ngConfirm({
            title: title,
            content: msg,
            type: typeclr,
            typeAnimated: true,
            buttons: {
                close: {
                    text: 'OK',
                    btnClass: btnclss,
                    action: function () {
                        if (title === 'Success !!')
                            location.reload();
                    }
                },
                OK: {
                    text: 'Copy To Sales Order',
                    btnClass: 'btn-success',
                    action: function () {
                        $scope.CopyQuotationToOrder();
                    }
                }
            }

        });
    };

    $scope.HideLoader = function () {
        angular.element('#loading').hide();
    };

    $scope.showloader = function () {
        angular.element('#loading').show();
    };


});

//-----------------------------------Directive For app----------------------------
app.directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9]/g, '');

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
}).directive('amountOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9.-]/g, '');

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
}).directive('compareTo', function () {
    return {
        require: 'ngModel',
        scope: {
            otherModelValue: "=compareTo"
        },
        link: function (scope, element, attributes, ngModel) {

            ngModel.$validators.compareTo = function (modelValue) {
                return modelValue === scope.otherModelValue;
            };

            scope.$watch("otherModelValue", function () {
                ngModel.$validate();
            });
        }
    };
}).directive("ngRightClick", function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngRightClick);
        element.bind("contextmenu", function (event) {
            scope.$apply(function () {
                event.preventDefault();
                fn(scope, { $event: event });
            });
        });
    };
}).filter('unique', function () {
    return function (collection, keyname) {
        var output = [],
            keys = [];
        angular.forEach(collection, function (item) {
            var key = item[keyname];
            if (keys.indexOf(key) === -1) {
                keys.push(key);
                output.push(item);
            }
        });
        return output;
    };
});

(function (angular) {
    var ngContextMenu = angular.module("directive.contextMenu", []);
    ngContextMenu.directive("cellHighlight", function () {
        return {
            restrict: "C",
            link: function postLink(scope, iElement, iAttrs) {
                iElement.find("td").mouseover(function () {
                    $(this).parent("tr").css("opacity", "0.7");
                }).mouseout(function () {
                    $(this).parent("tr").css("opacity", "1.0");
                });
            }
        };
    });

    ngContextMenu.directive("context", [
        function () {
            return {
                restrict: 'A',
                scope: "@&",
                compile: function compile(tElement, tAttrs, transclude) {
                    return {
                        post: function postLink(scope, iElement, iAttrs, controller) {
                            var ul = $('#' + iAttrs.context),
                                last = null;
                            ul.css({
                                "display": "none"
                            });
                            $(iElement).bind("contextmenu", function (event) {
                                event.preventDefault();
                                ul.css({
                                    position: "fixed",
                                    display: "block",
                                    left: event.clientX + "px",
                                    top: event.clientY + "px"
                                });
                                last = event.timeStamp;
                            });

                            $(document).click(function (event) {
                                var target = $(event.target);
                                if (!target.is(".popover") && !target.parents().is(".popover")) {
                                    if (last === event.timeStamp)
                                        return;
                                    ul.css({
                                        "display": "none"
                                    });
                                }
                            });
                        }
                    };
                }
            };
        }
    ]);
})(window.angular);



(function (angular) {
    "use strict";

    //function mainCtrl() {
    //    var vm = this;
    //    vm.numericValue = 12345678;
    //}

    function sgNumberInput($filter, $locale) {
        //#region helper methods
        function getCaretPosition(inputField) {
            // Initialize
            var position = 0;
            // IE Support
            if (document.selection) {
                inputField.focus();
                // To get cursor position, get empty selection range
                var emptySelection = document.selection.createRange();
                // Move selection start to 0 position
                emptySelection.moveStart('character', -inputField.value.length);
                // The caret position is selection length
                position = emptySelection.text.length;
            }
            else if (inputField.selectionStart || inputField.selectionStart === 0) {
                position = inputField.selectionStart;
            }
            return position;
        }
        function setCaretPosition(inputElement, position) {
            if (inputElement.createTextRange) {
                var range = inputElement.createTextRange();
                range.move('character', position);
                range.select();
            }
            else {
                if (inputElement.selectionStart) {
                    inputElement.focus();
                    inputElement.setSelectionRange(position, position);
                }
                else {
                    inputElement.focus();
                }
            }
        }
        function countNonNumericChars(value) {
            return (value.match(/[^a-z0-9]/gi) || []).length;
        }
        //#endregion helper methods

        return {
            require: "ngModel",
            restrict: "A",
            link: function ($scope, element, attrs, ctrl) {
                var fractionSize = parseInt(attrs['fractionSize']) || 0;
                var numberFilter = $filter('number');
                //format the view value
                ctrl.$formatters.push(function (modelValue) {
                    var retVal = numberFilter(modelValue, fractionSize);
                    var isValid = !isNaN(modelValue);
                    ctrl.$setValidity(attrs.name, isValid);
                    return retVal;
                });
                //parse user's input
                ctrl.$parsers.push(function (viewValue) {
                    var caretPosition = getCaretPosition(element[0]), nonNumericCount = countNonNumericChars(viewValue);
                    viewValue = viewValue || '';
                    //Replace all possible group separators
                    var trimmedValue = viewValue.trim().replace(/,/g, '').replace(/`/g, '').replace(/'/g, '').replace(/\u00a0/g, '').replace(/ /g, '');
                    //If numericValue contains more decimal places than is allowed by fractionSize, then numberFilter would round the value up
                    //Thus 123.109 would become 123.11
                    //We do not want that, therefore I strip the extra decimal numbers
                    var separator = $locale.NUMBER_FORMATS.DECIMAL_SEP;
                    var arr = trimmedValue.split(separator);
                    var decimalPlaces = arr[1];
                    if (decimalPlaces !== null && decimalPlaces.length > fractionSize) {
                        //Trim extra decimal places
                        decimalPlaces = decimalPlaces.substring(0, fractionSize);
                        trimmedValue = arr[0] + separator + decimalPlaces;
                    }
                    var numericValue = parseFloat(trimmedValue);
                    var isEmpty = numericValue === null || viewValue.trim() === "";
                    var isRequired = attrs.required || false;
                    var isValid = true;
                    if (isEmpty && isRequired || !isEmpty && isNaN(numericValue)) {
                        isValid = false;
                    }
                    ctrl.$setValidity(attrs.name, isValid);
                    if (!isNaN(numericValue) && isValid) {
                        var newViewValue = numberFilter(numericValue, fractionSize);
                        element.val(newViewValue);
                        var newNonNumbericCount = countNonNumericChars(newViewValue);
                        var diff = newNonNumbericCount - nonNumericCount;
                        var newCaretPosition = caretPosition + diff;
                        if (nonNumericCount === 0 && newCaretPosition > 0) newCaretPosition--;
                        setCaretPosition(element[0], newCaretPosition);
                    }
                    return !isNaN(numericValue) ? numericValue : null;
                });
            } //end of link function
        };
    }

    sgNumberInput.$inject = ["$filter", "$locale"];

    //angular
    //    .module("a", [])
    //    .controller("mainCtrl", mainCtrl)
    //    .directive("sgNumberInput", sgNumberInput);
    app.directive("sgNumberInput", sgNumberInput);
})(angular);