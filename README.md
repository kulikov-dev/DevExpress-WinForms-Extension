## DevExpress WinForms extension kit
  A small library to solve some troubles, which I met during my work with DevExpress.

### List of components
#### Editors
* CheckedListBoxControlDev. The extension for the standard component supports hotkeys for fast check/uncheck of items. Ctrl+A: check all, Ctrl+D: uncheck all, Ctrl+I: invert checking.
* ColorPickEditDev, RepositoryItemColorPickEdit. The extensions for the standard components allows to save custom user colors during the program, so each ColorPickEdit and RepositoryItemColorPickEdit will have the same, actual user colors.
* DateDoubleTrackbarControlDev. The visual component, which allows users to select range of dates: start, end and date between them. <p align="center"> <img src="DevExpressWinFormsExtension/Resources/Samples/DateDoubleTrackBarSample.jpg" width="230" align="center" title="Screenshot of the DateDoubleTrackbarControl"> </p>
* PasswordTextEditDev. The extension for working with passwords, allows user to show/hide input characters. <p align="center"> <img src="DevExpressWinFormsExtension/Resources/Samples/PasswordTextEditSample.jpg" width="150" align="center" title="Screenshot of the PasswordTextEdit"> </p>
* InputBoxValidableDev. The extension for working with passwords, allows user to show/hide input characters. <p align="center"> <img src="DevExpressWinFormsExtension/Resources/Samples/InputBoxValidableSample.jpg" width="250" align="center" title="Screenshot of the InputBoxValidable"> </p>
* GroupControlCheckedDev. TGroupControl with checkbox in header, allows user to disable/enable all controls in the GroupControl. <p align="center"> <img src="DevExpressWinFormsExtension/Resources/Samples/GroupControlCheckedSample.jpg" width="250" align="center" title="Screenshot of the GroupControlChecked"> </p>
* LookUpDev, RepositoryItemLookUpDev. The extension that allows to show hint for each element in the editor. <p align="center"> <img src="DevExpressWinFormsExtension/Resources/Samples/LookUpSample.jpg" width="320" align="center" title="Screenshot of the GroupControlChecked"> </p>

#### Extensions
* BaseEditExtension. Contains method 'IsValueEmpty' which allows validating input data of each BaseEdit control for emptiness. If necessary, set up the background color of the control to warning color.
* DateEditExtension. Contains method 'UpdateView' for DateEdit and RepositoryItemDateEdit to initialize the view of the control according to the datetime interval type. 
