<script lang="ts">
	import {onMount} from 'svelte';

	// 올바른 룬 사용법
	const allArticles = Array.from({length: 20}, (_, i) => ({
		id: i + 1,
		title: `게시물 제목 ${i + 1}`,
		description: `이것은 게시물 ${i + 1}의 내용입니다. 충분히 긴 설명을 통해 스크롤이 생기도록 합니다.`,
		date: new Date(Date.now() - i * 86400000).toLocaleDateString('ko-KR')
	}));

	// 참조용 변수
	let observerTarget: HTMLElement | null = $state(null);

	// 반응형 상태 정의
	let currentPage = $state(1);
	const itemsPerPage = 5;
	let loading = $state(false);

	// 파생 상태
	let visibleArticles = $derived(
			allArticles.slice(0, currentPage * itemsPerPage)
	);

	let hasMoreArticles = $derived(
			visibleArticles.length < allArticles.length
	);


	// 더 많은 게시물 로드 함수
	function loadMore() {
		loading = true;

		// 실제 환경에서는 API 호출이 있을 수 있으므로 약간의 지연 추가
		setTimeout(() => {
			if (visibleArticles.length < allArticles.length) {
				currentPage += 1;
			}
			loading = false;
		}, 300);
	}

	onMount(() => {
		// Intersection Observer를 사용한 무한 스크롤 구현
		if ('IntersectionObserver' in window && observerTarget) {
			const observer = new IntersectionObserver((entries) => {
				if (entries[0].isIntersecting && !loading && hasMoreArticles) {
					loadMore();
				}
			}, {rootMargin: '200px'});

			observer.observe(observerTarget);

			return () => observer.disconnect();
		}
	});
</script>

<div class="space-y-8">
	<section class="space-y-6">
		<h1 class="text-4xl font-bold">웹사이트에 오신 것을 환영합니다</h1>
		<p class="text-xl text-surface-700-200-token">
			이 페이지는 헤더의 sticky 기능을 테스트하기 위한 것입니다. 아래로 스크롤하면 헤더가 상단에 고정되는지 확인하세요.
		</p>
		<div class="h-1 w-1/2 bg-primary-500 rounded-full"></div>
	</section>

	<section class="space-y-6">
		<h2 class="text-3xl font-bold">최근 게시물</h2>
		<div class="grid grid-cols-1 gap-6">
			{#each visibleArticles as article (article.id)}
				<div class="card p-6 space-y-3 hover:bg-surface-100-800-token transition-colors">
					<div class="flex justify-between items-center">
						<h3 class="text-2xl font-bold">{article.title}</h3>
						<span class="text-sm text-surface-500-400-token">{article.date}</span>
					</div>
					<p>{article.description}</p>
					<a href={`/post/${article.id}`} class="btn variant-soft-primary">자세히 보기</a>
				</div>
			{/each}

			<!-- 로딩 상태 표시 -->
			{#if loading}
				<div class="p-4 text-center">
					<span class="animate-pulse">게시물을 불러오는 중...</span>
				</div>
			{/if}

			<!-- 더 불러올 게시물이 있을 때만 버튼 표시 -->
			{#if hasMoreArticles}
				<div class="flex justify-center p-4">
					<button
							onclick={loadMore}
							class="btn variant-soft-primary"
							disabled={loading}
					>
						더 많은 게시물 보기
					</button>
				</div>

				<!-- Intersection Observer 대상 요소 -->
				<div bind:this={observerTarget} class="h-1 w-full"></div>
			{/if}
		</div>
	</section>

	<section class="space-y-6 py-8">
		<h2 class="text-3xl font-bold">연락처</h2>
		<p>
			문의사항이 있으시면 아래 정보를 통해 연락해주세요.
		</p>
		<div class="flex flex-col space-y-2">
			<a class="hover:text-primary-500" href="mailto:example@example.com">example@example.com</a>
			<a class="hover:text-primary-500" href="tel:+82-10-1234-5678">+82-10-1234-5678</a>
		</div>
	</section>
</div>