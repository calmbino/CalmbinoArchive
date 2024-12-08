export function callHljs() {
    console.log("CallHljs");
    hljs.highlightAll();
}

export function changeLinkTarget() {
    document.querySelectorAll('a').forEach(link => {
        link.setAttribute('target', '_blank');
    });
}


