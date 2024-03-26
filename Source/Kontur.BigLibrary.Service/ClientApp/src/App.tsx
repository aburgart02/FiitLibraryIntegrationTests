import * as React from "react";
// import {GlobalErrorHandler} from "src/components/ErrorPage/GlobalErrorHandler";
import { Route, Switch } from "react-router-dom";
import MainPage from "src/components/MainPage/MainPage";
import BookViewPage from "src/components/BookViewPage/BookViewPage";
import HowItWorksPage from "src/components/HowItWorksPage/HowItWorksPage";
import { ServerErrorPage } from "src/components/ErrorPage/ServerErrorPage";
import { NotFoundPage } from "src/components/ErrorPage/NotFoundPage";
import { LoginForm } from "./components/AuthForm/LoginForm";
import { RegisterFormContainer } from "./components/AuthForm/RegisterFormContainer";
import { useAuthentication } from "./hooks/UseAuthentication";

const App: React.FC = () => {
    useAuthentication();

    return <>
        {/*<GlobalErrorHandler/>*/}
        <Switch>
            <Route path="/login" render={() => <LoginForm />} />
            <Route path="/register" render={() => <RegisterFormContainer />} />
            <Route exact path="/" render={() => <MainPage filter={{}} />} />
            <Route exact path="/query/:query" render={({ match }) => <MainPage filter={{ query: decodeURIComponent(match.params.query) }} />} />
            <Route exact path="/rubric/:rubricSynonym" render={({ match }) => <MainPage filter={{ rubricSynonym: match.params.rubricSynonym }} />} />
            <Route exact path={"/books/:synonym"} render={({ match }) => <BookViewPage synonym={match.params.synonym} />} />
            <Route exact path="/how-it-works" render={() => <HowItWorksPage />} />
            <Route exact path="/server-error" render={() => <ServerErrorPage />} />
            <Route render={() => <NotFoundPage />} />
        </Switch>
    </>;
};

export { App };