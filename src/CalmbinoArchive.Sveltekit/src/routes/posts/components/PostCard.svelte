<script lang="ts">
  import type {Post} from "../types/post";

  export let post: Post;

  export function formatDateKST(dateStr: string): string {
    const date = new Date(dateStr);

    const formatter = new Intl.DateTimeFormat('ko-KR', {
      timeZone: 'Asia/Seoul',
      year: 'numeric',
      month: 'numeric',
      day: 'numeric'
    });

    return `${formatter.format(date)} (KST)`;
  }

</script>

<a
    href="posts/{post.slug}"
    class="card preset-filled-surface-100-900 border border-surface-200-800 card-hover divide-y divide-surface-200-800 block  w-full rounded-xl overflow-hidden"
>
  <!-- Header (이미지 비율 고정) -->
  <header>
    <img
        src={post.imageUrl}
        alt="banner"
        class="aspect-[21/9] w-full object-cover grayscale hue-rotate-90"
    />
  </header>

  <!-- Article (본문 내용 제한) -->
  <article class="space-y-2 p-4">
    <div>
      <h2 class="text-sm font-semibold text-primary">{post.category}</h2>
      <h3 class="text-lg font-bold">{post.title}</h3>
    </div>
    <p class="opacity-60 line-clamp-3 text-sm">
      {post.description}
    </p>
  </article>

  <!-- Footer -->
  <footer class="flex items-center justify-between gap-4 p-4 text-xs text-muted">
    <small>By {post.author}</small>
    <small>On {formatDateKST(post.date)}</small>
  </footer>
</a>
