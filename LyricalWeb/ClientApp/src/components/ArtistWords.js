import React, { Component } from 'react';

export class ArtistWords extends Component {
    // TODO split the presentation and retrieval to separate modules/components
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            artistName: '',
            averageWordCount: null,
            error: null
        };
        this.artistNameChanged = this.artistNameChanged.bind(this);
        this.goClicked = this.goClicked.bind(this);
        this.fetchArtistWords = this.fetchArtistWords.bind(this);
    }

    artistNameChanged(e) {
        this.setState({
            ...this.state,
            artistName: e.target.value
        });
    }

    goClicked() {
        this.setState({ ...this.state, loading: true, error: null }, () => this.fetchArtistWords());
    }

    async fetchArtistWords() {
        const response = await window.fetch(`artist-words?artistName=${this.state.artistName}`);

        if (!response.ok) {
            this.setState({ ...this.state, loading: false, error: response.statusText });
            return;
        }

        const data = await response.json();
        console.log(data);

        this.setState({
            ...this.state,
            loading: false,
            averageWordCount: data.averageWordCount
        });
    }

    render() {
        return (
            <div>
                <p>Enter the name of an artist, then click Go:</p>

                <input placeholder="Artist Name" value={this.state.artistName} onChange={this.artistNameChanged} />&nbsp;
                <button className="btn btn-primary" onClick={this.goClicked}>Go</button>

                <p>&nbsp;</p>

                {this.state.error &&
                    <p style={{ color: 'red' }}>{this.state.error}</p>
                }

                {this.state.loading &&
                    <p><em>Loading...</em></p>
                }

                {!this.state.loading && this.state.averageWordCount &&
                    <p>Average number of words: {this.state.averageWordCount}</p>
                }
            </div>
        );
    }
}
