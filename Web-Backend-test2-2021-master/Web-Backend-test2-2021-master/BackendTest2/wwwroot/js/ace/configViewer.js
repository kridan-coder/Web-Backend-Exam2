$(function () {
    var editor = ace.edit("editor");
    var configs = localStorage.getItem("aceEditorConfig");
    if (configs !== null) {
        editor.setOptions(JSON.parse(configs));
    }
    window.onbeforeunload = function() {
        var internalEditor = ace.edit("editor");
        internalEditor.setReadOnly(false);
        localStorage.setItem("aceEditorConfig", JSON.stringify(internalEditor.getOptions()));
    };
    editor.setReadOnly(true);
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

function SetCode(sourceCode) {
    var editor = ace.edit("editor");
    editor.setReadOnly(true);
    editor.setValue(sourceCode, -1);
}

function UpdateSyntaxHighlighter() {
    var editor = ace.edit("editor");
    var selectedPrLang = $("#ProgrammingLanguage").text();
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
    case "python":
        return "python";
    case "pascal":
        return "pascal";
    default:
        return "c_cpp";
    }
}