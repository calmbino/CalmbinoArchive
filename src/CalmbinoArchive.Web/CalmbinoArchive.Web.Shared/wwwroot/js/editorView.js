export function callHljs() {
    console.log("CallHljs");
    hljs.highlightAll();
}

export function changeLinkTarget() {
    // 렌더링 뷰 내에 있는 a 태그에만 적용
    document.querySelectorAll('.editor-view a').forEach(link => {
        link.setAttribute('target', '_blank');
    });
}

export function addCopyFuncInCodeBlock() {
    // Add copy functionality for <pre><code></code></pre> only
    document.querySelectorAll('pre > code').forEach(codeBlock => {
        const pre = codeBlock.parentElement;
        if (pre && pre.tagName === 'PRE') {
            // Avoid duplicating buttons
            if (pre.querySelector('.copy-btn')) return;

            const button = document.createElement('button');
            button.textContent = 'Copy';
            button.className = 'copy-btn';
            pre.appendChild(button);

            button.addEventListener('click', () => {
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
            });
        }
    });
}
