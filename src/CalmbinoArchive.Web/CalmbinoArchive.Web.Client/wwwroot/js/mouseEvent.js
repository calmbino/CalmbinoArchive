export function addScrollListener(element, dotnetHelper) {

    if (!element) {
        console.error("Element is not defined.");
        return;
    }

    // Intersection Observer 생성
    const observer = new IntersectionObserver(
        (entries) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    console.log("Element is in view.");
                    dotnetHelper.invokeMethodAsync('LoadMoreItems');
                }
            });
        },
        {
            root: null, // 뷰포트를 기준으로
            rootMargin: "0px", // 뷰포트와의 여백
            threshold: 0.1 // 10% 이상 보이면 트리거
        }
    );

    // Observer에 요소 등록
    observer.observe(element);
}
