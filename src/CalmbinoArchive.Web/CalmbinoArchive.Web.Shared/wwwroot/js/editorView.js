export function callHljs() {
    console.log("CallHljs");
    hljs.highlightAll();
}

export function changeLinkTarget() {
    document.querySelectorAll('a').forEach(link => {
        link.setAttribute('target', '_blank');
    });
}

export function addCopyFuncInCodeBlock() {
// Add copy functionality for highlighted code blocks
    document.querySelectorAll('pre').forEach(pre => {
        const button = document.createElement('button');
        button.textContent = 'Copy';
        button.className = 'copy-btn';
        pre.appendChild(button);

        button.addEventListener('click', () => {
            const codeBlock = pre.querySelector('code');
            if (codeBlock) {
                const text = codeBlock.textContent;

                // Copy to clipboard
                navigator.clipboard.writeText(text).then(() => {
                    button.textContent = 'Copied!';

                    // Reset button text after a delay
                    setTimeout(() => {
                        button.textContent = 'Copy';
                    }, 2000);
                }).catch(err => {
                    console.error('Failed to copy text: ', err);
                });
            }
        });
    });
}
