public class RC4 {

    private byte[] key;
    private int[] S;

    public RC4(String key) {
        this.key = key.getBytes();
        initialize();
    }

    private void initialize() {
        S = new int[256];
        for (int i = 0; i < 256; i++) {
            S[i] = i;
        }

        int j = 0;
        for (int i = 0; i < 256; i++) {
            j = (j + S[i] + key[i % key.length]) % 256;
            swap(i, j);
        }
    }

    private void swap(int i, int j) {
        int temp = S[i];
        S[i] = S[j];
        S[j] = temp;
    }

    public byte[] encrypt(byte[] data) {
        byte[] ciphertext = new byte[data.length];
        int i = 0;
        int j = 0;

        for (int k = 0; k < data.length; k++) {
            i = (i + 1) % 256;
            j = (j + S[i]) % 256;
            swap(i, j);
            int t = (S[i] + S[j]) % 256;
            ciphertext[k] = (byte) (data[k] ^ S[t]);
        }

        return ciphertext;
    }

    public byte[] decrypt(byte[] data) {
        byte[] plaintext = new byte[data.length];
        int i = 0;
        int j = 0;

        for (int k = 0; k < data.length; k++) {
            i = (i + 1) % 256;
            j = (j + S[i]) % 256;
            swap(i, j);
            int t = (S[i] + S[j]) % 256;
            plaintext[k] = (byte) (data[k] ^ S[t]);
        }

        return plaintext;
    }
}