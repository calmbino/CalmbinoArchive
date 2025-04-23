import type { Post } from './types/post';

export const posts: Post[] = [
	{
		slug: 'skeleton-is-awesome',
		imageUrl:
			'https://images.unsplash.com/photo-1515879218367-8466d910aaa4?w=800&h=600&fit=crop',
		category: 'Announcements',
		title: 'Skeleton is Awesome',
		description:
			'Lorem ipsum dolor sit amet consectetur adipisicing elit. Numquam aspernatur provident eveniet eligendi.',
		author: 'Alex',
		date: '2025-04-23T00:00:00+09:00'
	},
	{
		slug: 'exploring-sveltekit-2',
		imageUrl:
			'https://images.unsplash.com/photo-1508780709619-79562169bc64?w=800&h=600&fit=crop',
		category: 'Tech',
		title: 'Exploring SvelteKit 2.0',
		description:
			'SvelteKit 2.0 brings powerful updates for SSR and edge rendering. Here’s what you need to know.',
		author: 'Jamie',
		date: '2025-04-20T00:00:00+09:00'
	},
	{
		slug: 'why-clean-architecture-matters',
		imageUrl:
			'https://images.unsplash.com/photo-1508780709619-79562169bc64?w=800&h=600&fit=crop',
		category: 'Development',
		title: 'Why Clean Architecture Matters',
		description:
			'Clean Architecture separates concerns and increases maintainability. We break it down for beginners.',
		author: 'Sam',
		date: '2025-04-18T00:00:00+09:00'
	},
	{
		slug: 'top-10-extensions-for-web-devs',
		imageUrl:
			'https://images.unsplash.com/photo-1519389950473-47ba0277781c?w=800&h=600&fit=crop',
		category: 'Tools',
		title: 'Top 10 Extensions for Web Devs',
		description:
			'Boost your productivity with these essential VSCode extensions for frontend and backend developers.',
		author: 'Morgan',
		date: '2025-04-15T00:00:00+09:00'
	},
	{
		slug: 'dockerize-your-aspnet-app',
		imageUrl:
			'https://images.unsplash.com/photo-1519389950473-47ba0277781c?w=800&h=600&fit=crop',
		category: 'Tutorials',
		title: 'Dockerize Your ASP.NET App',
		description:
			'A step-by-step guide to containerize and deploy your ASP.NET Core app using Docker.',
		author: 'Taylor',
		date: '2025-04-10T00:00:00+09:00'
	},
	{
		slug: 'lessons-from-side-projects',
		imageUrl:
			'https://images.unsplash.com/photo-1504384308090-c894fdcc538d?w=800&h=600&fit=crop',
		category: 'Inspiration',
		title: 'Lessons from Building Side Projects',
		description:
			'Side projects are more than code. Discover what developers learn from building something on their own.',
		author: 'Jordan',
		date: '2025-04-05T00:00:00+09:00'
	}
];
