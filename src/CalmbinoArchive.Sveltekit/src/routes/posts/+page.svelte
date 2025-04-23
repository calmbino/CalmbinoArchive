<script lang="ts">
  import type {PageProps} from './$types';
  import PostCard from "./components/PostCard.svelte";

  let {data}: PageProps = $props();
</script>

<div class="grid gap-3 grid-cols-1 lg:grid-cols-2 xl:grid-cols-3 max-w-7xl mx-auto">
  {#each data.posts as post (post.slug)}
    <PostCard {post}/>
  {/each}
</div>