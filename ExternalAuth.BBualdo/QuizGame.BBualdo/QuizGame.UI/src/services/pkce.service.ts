import { Injectable } from '@angular/core';
import { sha256 } from 'crypto-hash';

@Injectable({
  providedIn: 'root',
})
export class PkceService {
  async generateCodeVerifierAndChallenge() {
    sessionStorage.clear();
    const codeChallenge = this.generateRandomString(50);
    // const codeChallenge = await this.generateCodeChallenge(codeVerifier);
    sessionStorage.setItem('codeVerifier', codeChallenge);

    return codeChallenge;
  }

  private generateRandomString(length: number) {
    const possible =
      'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~';
    let text = '';
    for (let i = 0; i < length; i++) {
      text += possible.charAt(Math.floor(Math.random() * possible.length));
    }
    return text;
  }

  private async generateCodeChallenge(codeVerifier: string) {
    const base64 = await sha256(codeVerifier);
    return base64.replace(/\+/g, '-').replace(/\//g, '_').replace(/=/g, '');
  }
}
