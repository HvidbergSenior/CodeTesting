import type { ErrorInfo, ReactNode } from "react";
import { Component, Fragment } from "react";

interface Props {
  children: ReactNode;
}

interface State {
  hasError: boolean;
  message?: string;
}

export class ErrorBoundary extends Component<Props, State> {
  public state: State = {
    hasError: false,
  };

  public static getDerivedStateFromError(error: Error): State {
    return { hasError: true, message: error.message };
  }

  public componentDidCatch(error: Error, errorInfo: ErrorInfo) {
    console.error("Uncaught error:", error, errorInfo);
  }

  public render() {
    if (this.state.hasError) {
      return (
        <Fragment>
          <h1>Sorry... there was an error</h1>
          <pre>{this.state.message}</pre>
        </Fragment>
      );
    }

    return this.props.children;
  }
}
