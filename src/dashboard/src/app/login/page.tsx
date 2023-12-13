'use client';

import { Button } from '@/components';
import { signIn } from 'next-auth/react';
import styles from './Login.module.scss'

export default function Page() {
  return (
    <div className={`dashboardContainer ${styles.container}`}>
      <div className={styles.welcome}>
        <h1>Welcome to the Storage Dashboard!</h1>
        <p>Login to get insights into your organization&apos;s data storage and usage.</p>
      </div>
      <div className={styles.login}>
        <div className={styles.panel}>
          <h2>Login</h2>
          <Button variant="primary" onClick={() => signIn('keycloak')} title="Sign in">
            Sign in
          </Button>
        </div>
        <div>
          <h3>Need access to the Storage Dashboard?</h3>
          <p>
            Please email <a href="mailto:placeholder@gov.bc.ca">placeholder@gov.bc.ca</a> to request
            access to your organization&apos;s dashboard.
          </p>
          <h3>If you are a first time user please note:</h3>
          <ul>
            <li>
              Your first login will include a registration step within the BCGov Single Sign-On
              system.
            </li>
            <li>
              As part of the registration process, you&apos;ll be asked to provide your business
              contact information and verify your email.
            </li>
          </ul>
        </div>
      </div>
    </div>
  );
}
