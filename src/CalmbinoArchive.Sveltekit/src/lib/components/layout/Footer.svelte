<script>
  import {Github, Linkedin} from '@lucide/svelte';
  import {archiveCopyright} from '$lib/config.js';
</script>

<footer class="border-t border-surface-300-600-token p-4 text-sm text-surface-700-200-token">
  <div class="mx-auto flex max-w-[90%] flex-col items-center justify-between gap-2 sm:flex-row">
    <span>{archiveCopyright}</span>

    <div class="flex gap-3">
      <a
          href="https://github.com/calmbino"
          target="_blank"
          rel="noopener noreferrer"
          class="btn-icon variant-ghost-surface hover:variant-soft-primary"
          title="GitHub"
      >
        <Github size={18} />
      </a>

      <a
          href="https://linkedin.com/in/your-id"
          target="_blank"
          rel="noopener noreferrer"
          class="btn-icon variant-ghost-surface hover:variant-soft-primary"
          title="LinkedIn"
      >
        <Linkedin size={18} />
      </a>
    </div>
  </div>
</footer>