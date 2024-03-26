const image1 = require( "../storage/images/1.jpg");
const image2 = require( "../storage/images/2.jpg");
const image3 = require( "../storage/images/3.jpg");
const image4 = require( "../storage/images/4.jpg");
const image5 = require( "../storage/images/5.jpg");
const image6 = require( "../storage/images/6.jpg");

const images = [image1, image2, image3, image4, image5, image6];

const getImageUrl = (id: number): string => {
    return images[id % 3];
};

export const fakeImageApi = {
    getUrl: getImageUrl,
};

