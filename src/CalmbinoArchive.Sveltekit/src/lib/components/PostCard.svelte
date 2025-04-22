<script lang="ts">
  const imgSrc =
    'https://images.unsplash.com/photo-1463171515643-952cee54d42a?q=80&w=450&h=190&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D';
</script>

<a
    href="posts/1"
    class="card preset-filled-surface-100-900 border border-surface-200-800 card-hover divide-y divide-surface-200-800 block  w-full rounded-xl overflow-hidden"
>
  <!-- Header (이미지 비율 고정) -->
  <header>
    <img
        src={imgSrc}
        alt="banner"
        class="aspect-[21/9] w-full object-cover grayscale hue-rotate-90"
    />
  </header>

  <!-- Article (본문 내용 제한) -->
  <article class="space-y-2 p-4">
    <div>
      <h2 class="text-sm font-semibold text-primary">Announcements</h2>
      <h3 class="text-lg font-bold">Skeleton is Awesome</h3>
    </div>
    <p class="opacity-60 line-clamp-3 text-sm">
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Numquam aspernatur provident eveniet eligendi cumque consequatur tempore sint nisi sapiente. Iste beatae laboriosam iure molestias cum expedita architecto itaque quae rem.
    </p>
  </article>

  <!-- Footer -->
  <footer class="flex items-center justify-between gap-4 p-4 text-xs text-muted">
    <small>By Alex</small>
    <small>On {new Date().toLocaleDateString()}</small>
  </footer>
</a>
