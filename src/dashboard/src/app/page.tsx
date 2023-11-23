import Link from 'next/link';

export default function Page() {
  return (
    <main>
      <div>
        <Link href="/login">Go to Login Page</Link>
      </div>
      <div>Welcome Home Page</div>
    </main>
  );
}
