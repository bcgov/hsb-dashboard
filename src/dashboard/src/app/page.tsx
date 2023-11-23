import Link from 'next/link';

export default function Page() {
  return (
    <main>
      <div>
        <Link href="/login">Login Page</Link>
      </div>
      <div>Welcome Home</div>
    </main>
  );
}
