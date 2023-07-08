"""Color match

Opens a PNG file and rounds colors to match the color palette
"""
from PIL import Image
import numpy as np

# Color palette
palette = np.array([
    [23, 32, 56],
    [37, 58, 94],
    [60, 94, 139],
    [79, 143, 186],
    [115, 190, 211],
    [164, 221, 219],
    [25, 51, 45],
    [37, 86, 46],
    [70, 130, 50],
    [17, 167, 67],
    [168, 202, 88],
    [218, 208, 145],
    [77, 43, 50],
    [122, 72, 65],
    [173, 119, 87],
    [192, 148, 115],
    [215, 181, 148],
    [231, 215, 179],
    [52, 28, 39],
    [96, 44, 44],
    [136, 75, 43],
    [190, 119, 43],
    [222, 158, 65],
    [232, 193, 112],
    [36, 21, 39],
    [65, 29, 49],
    [117, 36, 56],
    [165, 48, 48],
    [207, 87, 60],
    [218, 134, 62],
    [30, 29, 57],
    [64, 39, 81],
    [122, 54, 123],
    [162, 62, 140],
    [198, 81, 151],
    [223, 132, 165],
    [9, 10, 20],
    [16, 20, 31],
    [21, 29, 40],
    [32, 46, 55],
    [57, 74, 80],
    [87, 114, 119],
    [129, 151, 150],
    [168, 181, 178],
    [199, 207, 204],
    [235, 237, 233]
])


def closest_color(rgba: np.ndarray) -> np.ndarray:
    r, g, b, a = rgba
    color_diffs = np.sum((palette - [r, g, b]) ** 2, axis=1)
    return palette[np.argmin(color_diffs)]


if __name__ == '__main__':
    # Open image
    im = Image.open("tiles_single_rawcolor.png")
    im = im.convert("RGBA")

    data = np.array(im)

    # Find closest color in palette
    new_data = np.zeros(data.shape)
    for i in range(data.shape[0]):
        for j in range(data.shape[1]):
            new_data[i, j, :3] = closest_color(data[i, j])
    new_data[:, :, 3] = data[:, :, 3]

    # Save image
    im2 = Image.fromarray(new_data.astype(np.uint8))
    im2.save("tiles_single_color.png")
