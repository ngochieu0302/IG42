CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
    // Define changes to default configuration here. For example:
    config.language = 'vi';
    config.extraPlugins = 'youtube';
    config.allowedContent = true;
    config.entities_latin = false;
	config.height = 500;
	config.height = '25em';
	config.height = '300px';
    $.each(CKEDITOR.dtd.$removeEmpty, function (i, value) {
        CKEDITOR.dtd.$removeEmpty[i] = false;
    });
    // config.uiColor = '#AADC6E';
    //  config.toolbar =
    //[
    //   ['Source', '-', 'Bold', 'Italic', 'syntaxhighlight']
    //];
    config.filebrowserBrowseUrl = '/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/ckfinder/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = '/ckfinder/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';

    config.toolbar = 'Custom';

    config.toolbar_Custom = [
		['Source'],
		['Maximize'],
		['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
		['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
		['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
		['SpecialChar'],
		'/',
		['Undo', 'Redo'],
		['Font', 'FontSize'],
		['TextColor', 'BGColor'],
		['Link', 'Unlink', 'Anchor'],
		['Image', 'Youtube', 'Table', 'HorizontalRule']
    ];

    config.toolbar_Full =
	[
		['Source', '-', 'Save', 'NewPage', 'Preview', '-', 'Templates'],
		['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print', 'SpellChecker', 'Scayt'],
		['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
		['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'],
		'/',
		['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
		['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'],
		['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
		['Link', 'Unlink', 'Anchor'],
		['Image', 'Flash', 'Youtube', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak'],
		'/',
		['Styles', 'Format', 'Font', 'FontSize'],
		['TextColor', 'BGColor'],
		['Maximize', 'ShowBlocks', '-', 'About']
	];
};

// allow i tags to be empty (for font awesome)
CKEDITOR.dtd.$removeEmpty['div'] = false;
CKEDITOR.dtd.$removeEmpty['a'] = false;
CKEDITOR.dtd.$removeEmpty['i'] = false;
CKEDITOR.dtd.$removeEmpty['span'] = false;