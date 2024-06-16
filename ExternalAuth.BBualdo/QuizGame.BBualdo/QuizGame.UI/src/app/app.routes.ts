import { Routes } from '@angular/router';
import { MainMenuComponent } from '../components/main-menu/main-menu.component';
import { QuizManagerLayout } from '../components/Quiz-Management/quiz-manager-layout/quiz-manager.component';
import { QuizDetailsComponent } from '../components/Quiz-Management/quiz-details/quiz-details.component';
import { QuizListComponent } from '../components/Quiz-Management/quiz-list/quiz-list.component';
import { CreateQuizComponent } from '../components/Quiz-Management/Quiz-Creator/create-quiz/create-quiz.component';
import { StepperComponent } from '../components/Quiz-Management/Quiz-Creator/stepper/stepper.component';
import { SelectQuizComponent } from '../components/Game-Session/select-quiz/select-quiz.component';
import { CreateGameLayout } from '../components/Game-Session/create-game-layout/create-game-layout.component';
import { SelectDifficultyComponent } from '../components/Game-Session/select-difficulty/select-difficulty.component';
import { GameSessionComponent } from '../components/Game-Session/game-session/game-session.component';
import { GameResultsComponent } from '../components/Game-Session/game-results/game-results.component';
import { LeaderboardComponent } from '../components/Leaderboard/leaderboard/leaderboard.component';
import { LoginComponent } from '../components/Auth/login/login.component';
import { SignUpComponent } from '../components/Auth/sign-up/sign-up.component';
import { AuthLayoutComponent } from '../components/Auth/auth-layout/auth-layout.component';
import { GoogleCallbackComponent } from '../components/ExternalAuth/google-callback/google-callback.component';
import { FacebookCallbackComponent } from '../components/ExternalAuth/facebook-callback/facebook-callback.component';
import { MicrosoftCallbackComponent } from '../components/ExternalAuth/microsoft-callback/microsoft-callback.component';
import { GithubCallbackComponent } from '../components/ExternalAuth/github-callback/github-callback.component';
import { TwitterCallbackComponent } from '../components/ExternalAuth/twitter-callback/twitter-callback.component';

export const routes: Routes = [
  { path: '', component: MainMenuComponent },
  // Session Routes
  {
    path: 'play',
    component: CreateGameLayout,
    children: [
      { path: '', component: SelectQuizComponent },
      { path: 'difficulty', component: SelectDifficultyComponent },
      { path: 'session', component: GameSessionComponent },
      { path: 'results', component: GameResultsComponent },
    ],
  },
  // Quiz Management Routes
  {
    path: 'quiz-management',
    component: QuizManagerLayout,
    children: [
      { path: '', component: QuizListComponent },
      {
        path: 'create',
        children: [
          { path: '', component: CreateQuizComponent },
          { path: 'steps', component: StepperComponent },
        ],
      },
      { path: ':id', component: QuizDetailsComponent },
    ],
  },
  // Leaderboard route
  {
    path: 'leaderboard',
    component: LeaderboardComponent,
  },
  // Auth Routes
  {
    path: 'auth',
    component: AuthLayoutComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: SignUpComponent },
      { path: 'signin-google', component: GoogleCallbackComponent },
      { path: 'signin-facebook', component: FacebookCallbackComponent },
      { path: 'signin-microsoft', component: MicrosoftCallbackComponent },
      { path: 'signin-github', component: GithubCallbackComponent },
      { path: 'signin-twitter', component: TwitterCallbackComponent },
    ],
  },
];
