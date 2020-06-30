import React, { Component } from 'react';
import { ArtistWords } from './ArtistWords';

export class Home extends Component {
    render() {
        return (
            <div>
                <h1>Lyrical</h1>
                <ArtistWords />
            </div>
        );
    }
}