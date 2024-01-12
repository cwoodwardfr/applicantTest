import javax.net.ssl.*;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.security.cert.X509Certificate;

public class ApplicantTest {
    private static final String uniqueURL = "<YOU MUST CHANGE THIS>";

    public static void main(String[] args) {
        int iterations = 5;
        int number = 10000;
        long totalTime = 0;
        try {
            // Key for RC4 encryption
            String key = "cafebabeistheperfectkey";
            RC4 rc4 = new RC4(key);

            for (int i = 0; i < iterations; i++) {
                String[] randomStrings = getRandomStrings(number);
                long startTime = System.currentTimeMillis();
                for (String randomString : randomStrings) {
                    byte[] data = randomString.getBytes();
                    byte[] encryptedData = rc4.encrypt(data);
                }
                totalTime += System.currentTimeMillis() - startTime;
            }

            System.out.println("Processed " + iterations*number + " samples.");
            System.out.println("Total time: " + totalTime + " ms.");
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private static String[] getRandomStrings(int count) throws IOException {
        String[] randomStrings = new String[count];
        String s = getRandomString(uniqueURL);

        for (int i = 0; i < count; i++) {
            // Fetch a random string from the URL
            randomStrings[i] = s;
        }

        return randomStrings;
    }

    private static String getRandomString(String url) throws IOException {
        StringBuilder result = new StringBuilder();
        HttpURLConnection connection = null;

        try {
            disableSSLVerification();
            URL randomUrl = new URL(url);
            connection = (HttpURLConnection) randomUrl.openConnection();
            connection.setRequestMethod("GET");
            int responseCode = connection.getResponseCode();

            // If the response code is 200 (OK), read the data
            if (responseCode == HttpURLConnection.HTTP_OK) {
                BufferedReader reader = new BufferedReader(new InputStreamReader(connection.getInputStream()));
                String line;
                while ((line = reader.readLine()) != null) {
                    result.append(line);
                }
                reader.close();
            } else {
                System.out.println("Error: Unable to fetch random string. Response Code: " + responseCode);
            }
        } catch (NoSuchAlgorithmException | KeyManagementException e) {
            throw new RuntimeException(e);
        } finally {
            // Close the connection
            if (connection != null) {
                connection.disconnect();
            }
        }
        return result.toString();
    }

    private static void disableSSLVerification() throws NoSuchAlgorithmException, KeyManagementException {
        // Coping with Let's Encrypt vs Java cert store...
        TrustManager[] trustAllCerts = new TrustManager[]{
                new X509TrustManager() {
                    public X509Certificate[] getAcceptedIssuers() {
                        return null;
                    }

                    public void checkClientTrusted(X509Certificate[] certs, String authType) {
                    }

                    public void checkServerTrusted(X509Certificate[] certs, String authType) {
                    }
                }
        };

        // Install the all-trusting trust manager
        SSLContext sslContext = SSLContext.getInstance("SSL");
        sslContext.init(null, trustAllCerts, new SecureRandom());
        HttpsURLConnection.setDefaultSSLSocketFactory(sslContext.getSocketFactory());

        // Create an all-trusting host name verifier
        HostnameVerifier allHostsValid = (hostname, session) -> true;

        // Install the all-trusting host verifier
        HttpsURLConnection.setDefaultHostnameVerifier(allHostsValid);
    }
}