<script>
  import {archiveTitle} from '$lib/config.js';
  import {onMount} from 'svelte';

  // 스크롤 위치에 따라 헤더 스타일 변경
  let scrollY = 0;
  let isScrolled = false;

  // 컴포넌트가 마운트되면 스크롤 이벤트 리스너 추가
  onMount(() => {
    const handleScroll = () => {
      scrollY = window.scrollY;
      isScrolled = scrollY > 5; // 5px 이상 스크롤되면 스타일 변경
    };

    window.addEventListener('scroll', handleScroll);

    // 초기 스크롤 상태 확인
    handleScroll();

    // 컴포넌트가 언마운트될 때 이벤트 리스너 제거
    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  });
</script>

<header class="sticky top-0 z-10 border-b border-surface-300-600-token shadow-md p-4 transition-all duration-300 {isScrolled ? 'bg-surface-100-800-token/85 backdrop-blur-lg' : 'bg-transparent'}">
  <div class="mx-auto flex max-w-[90%] flex-col sm:flex-row justify-between items-center gap-2">
    <div>
      <a href="/" class="text-xl font-bold transition-colors hover:text-primary-500">
        {archiveTitle}
      </a>
    </div>

    <nav class="flex gap-1">
      <a
          href="/about"
          class="btn-icon variant-ghost-surface hover:variant-soft-primary w-16 flex justify-center"
          title="About"
      >
        About
      </a>
      <a
          href="/posts"
          class="btn-icon variant-ghost-surface hover:variant-soft-primary w-16 flex justify-center"
          title="Posts"
      >
        Posts
      </a>
    </nav>
  </div>
</header>