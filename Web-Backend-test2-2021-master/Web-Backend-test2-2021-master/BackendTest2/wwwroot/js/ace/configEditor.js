$(function() {
    var editor = ace.edit("editor");
    var configs = localStorage.getItem("aceEditorConfig");
    if (configs !== null) {
        editor.setOptions(JSON.parse(configs));
    }
    window.onbeforeunload = function () {
        var internalEditor = ace.edit("editor");
        localStorage.setItem("aceEditorConfig", JSON.stringify(internalEditor.getOptions()));
    }
    var sourceCode = $('[name="SourceCode"]').val();
    if (sourceCode !== null && sourceCode !== "") {
        editor.setValue(sourceCode, -1);
        $('[name="SourceCode"]').val("");
    }
    UpdateSyntaxHighlighter();
});

function OpenEditorFullScreen() {
    var editor = ace.edit("editor");
    editor.container.webkitRequestFullscreen();
};

function OpenSettingsMenu() {
    var editor = ace.edit("editor");
    editor.execCommand("showSettingsMenu");
}

function GetSourceCode() {
    var editor = ace.edit("editor");
    return editor.getValue();
}

function UpdateSyntaxHighlighter() {
    var editor = ace.edit("editor");
    var selectedPrLang = $("#ProgramingLanguage option:selected").text();
    editor.session.setMode("ace/mode/" + MapPrLangToAceMode(selectedPrLang));
}

function MapPrLangToAceMode(prLangName) {
    switch ((prLangName.split(" ")[0]).toLowerCase()) {
        case "c":
        case "c++":
            return "c_cpp";
        case "c#":
            return "csharp";
        case "java":
            return "java";
        case "python2":
        case "python3":
            return "python";
        case "pascal":
            return "pascal";
        default:
            return "c_cpp";
    }
}

function PasteExample() {
    var selectedPrLang = $("#ProgramingLanguage option:selected").text();
    var editor = ace.edit("editor");
    var t = GetExample(selectedPrLang);
    editor.setValue(t, -1);
}