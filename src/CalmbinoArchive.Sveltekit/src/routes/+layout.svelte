<script>
	import '../app.css';

	import Header from '$lib/components/layout/Header.svelte';
	import Footer from '$lib/components/layout/Footer.svelte';

	let { children } = $props();
</script>

<div class="min-h-screen flex flex-col">
	<Header/>

	<main class="flex-grow max-w-[65%] w-full mx-auto px-4 py-8">
		{@render children?.()}
	</main>

	<Footer/>
</div>